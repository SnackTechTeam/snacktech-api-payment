using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Configuration;
using common.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace unit_tests.api.configuration
{
    public class MongoDbExtensionsTests
    {
        [Fact]
        public void AdicionarMongoDBIMongoClientComSucesso()
        {
            var services = new ServiceCollection();
            var mongoDbOptions = new MongoDbOptions
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "TestDatabase"
            };
            var optionsMock = new Mock<IOptions<MongoDbOptions>>();
            optionsMock.Setup(o => o.Value).Returns(mongoDbOptions);

            services.AddSingleton(optionsMock.Object);

            services.AddMongoDB();
            var serviceProvider = services.BuildServiceProvider();

            var mongoClient = serviceProvider.GetService<IMongoClient>();
            Assert.NotNull(mongoClient);
        }

        [Fact]
        public void AdicionarMongoDBIMongoDatabaseComSucesso()
        {
            var services = new ServiceCollection();
            var mongoDbOptions = new MongoDbOptions
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "TestDatabase"
            };
            var optionsMock = new Mock<IOptions<MongoDbOptions>>();
            optionsMock.Setup(o => o.Value).Returns(mongoDbOptions);

            services.AddSingleton(optionsMock.Object);

            services.AddMongoDB();
            var serviceProvider = services.BuildServiceProvider();

            var mongoDatabase = serviceProvider.GetService<IMongoDatabase>();
            Assert.NotNull(mongoDatabase);
        }

        [Fact]
        public void AdicionarMongoDBUsandoConnectionStringComSucesso()
        {
            var services = new ServiceCollection();
            var mongoDbOptions = new MongoDbOptions
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "TestDatabase"
            };
            var optionsMock = new Mock<IOptions<MongoDbOptions>>();
            optionsMock.Setup(o => o.Value).Returns(mongoDbOptions);

            services.AddSingleton(optionsMock.Object);

            services.AddMongoDB();
            var serviceProvider = services.BuildServiceProvider();

            var mongoClient = serviceProvider.GetService<IMongoClient>();
            Assert.NotNull(mongoClient);
            Assert.Equal("localhost:27017", mongoClient.Settings.Server.ToString());
        }

        [Fact]
        public void AdicionarMongoDBUsandoDadosSeConnectionNaoForConfigurada()
        {
            // Arrange
            var services = new ServiceCollection();
            var mongoDbOptions = new MongoDbOptions
            {
                UserName = "user",
                Password = "password",
                Endpoint = "localhost",
                Port = "27017",
                DatabaseName = "TestDatabase",
                SSL = false
            };
            var optionsMock = new Mock<IOptions<MongoDbOptions>>();
            optionsMock.Setup(o => o.Value).Returns(mongoDbOptions);

            services.AddSingleton(optionsMock.Object);

            // Act
            services.AddMongoDB();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var mongoClient = serviceProvider.GetService<IMongoClient>();
            Assert.NotNull(mongoClient);
            Assert.Equal("localhost:27017", mongoClient.Settings.Server.ToString());
        }
    }
}