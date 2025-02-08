
using common.Interfaces;
using driver.database.mongo;
using driver.database.mongo.Entities;
using driver.database.mongo.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;

namespace unit_tests.databasemongo
{
    public class ModuleInjectionDependencyTest
    {
        [Fact]
        public void InjecaoDependenciaFeitaCorretamente()
        {
            var mongoDatabaseMock = new Mock<IMongoDatabase>();
            var mongoCollectionMock = new Mock<IMongoCollection<Pagamento>>();
            var mongoClientMock = new Mock<IMongoClient>();
            
            mongoCollectionMock.Setup(c => c.Indexes).Returns(new Mock<IMongoIndexManager<Pagamento>>().Object);
            mongoClientMock.Setup(c => c.GetDatabase("Test",null)).Returns(mongoDatabaseMock.Object);
            mongoDatabaseMock.Setup(d => d.GetCollection<Pagamento>("Pagamentos",null)).Returns(mongoCollectionMock.Object);

            var services = new ServiceCollection();
            services.AddScoped<IMongoDatabase>(sp => mongoDatabaseMock.Object);
            services.AddMongoDbService();

            var serviceProvider = services.BuildServiceProvider();

            var service = serviceProvider.GetService<IMongoDbIntegration>();
            Assert.NotNull(service);
            Assert.IsType<PagamentoRepository>(service);
        }
    }
}