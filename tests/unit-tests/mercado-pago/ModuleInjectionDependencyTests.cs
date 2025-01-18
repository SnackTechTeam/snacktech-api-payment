using common.Interfaces;
using driver.mercado_pago;
using driver.mercado_pago.services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace unit_tests.mercadopago
{
    public class ModuleInjectionDependencyTests
    {
        [Fact]
        public void InjecaoDeDependenciaFeitaCorretamente()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var services = new ServiceCollection();
            services.AddSingleton(httpClientFactoryMock.Object);

            services.AddMercadoPagoService();

            var serviceProvider = services.BuildServiceProvider();

            var service = serviceProvider.GetService<IMercadoPagoIntegration>();
            Assert.NotNull(service);
            Assert.IsType<MercadoPagoService>(service);
        }
    }
}