
using api.Configuration;
using common.Interfaces;
using common.Options;
using core.controllers;
using core.interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace unit_tests.api.configuration
{
    public class DomainInjectionDependencyTests
    {
        [Fact]
        public void AdicionarDomainControllerComSucesso()
        {
            var services = new ServiceCollection();
            var mercadoPagoIntegrationMock = new Mock<IMercadoPagoIntegration>();
            var mongoDbIntegrationMock = new Mock<IMongoDbIntegration>();
            var sqsIntegrationMock = new Mock<ISqsIntegration>();
            var mercadoPagoOptionsMock = new Mock<IOptions<MercadoPagoOptions>>();

            services.AddScoped<IMercadoPagoIntegration>(sp => mercadoPagoIntegrationMock.Object);
            services.AddScoped<IMongoDbIntegration>(sp => mongoDbIntegrationMock.Object);
            services.AddScoped<ISqsIntegration>(sp => sqsIntegrationMock.Object);
            services.AddScoped<IOptions<MercadoPagoOptions>>(sp => mercadoPagoOptionsMock.Object);


            services.AddDomainControllers();
            var serviceProvider = services.BuildServiceProvider();
            var controller = serviceProvider.GetService<IPagamentoController>();

            Assert.NotNull(controller);
            Assert.IsType<PagamentoController>(controller);
        }
    }
}