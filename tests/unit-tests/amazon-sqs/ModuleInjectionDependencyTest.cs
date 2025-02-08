
using common.Interfaces;
using driver.amazon.sqs;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace unit_tests.amazonsqs
{
    public class ModuleInjectionDependencyTest
    {
       [Fact]
        public void AddSqsServiceDeveRegistrarServicoComoEsperado()
        {
            var services = new Mock<IServiceCollection>();
            var serviceDescriptor = new ServiceDescriptor(typeof(ISqsIntegration), typeof(SqsService), ServiceLifetime.Transient);

            services.Setup(s => s.Add(It.Is<ServiceDescriptor>(sd => 
                sd.ServiceType == serviceDescriptor.ServiceType &&
                sd.ImplementationType == serviceDescriptor.ImplementationType &&
                sd.Lifetime == serviceDescriptor.Lifetime)))
                .Verifiable();

            services.Object.AddSqsService();

            services.Verify();
        }
    }
}