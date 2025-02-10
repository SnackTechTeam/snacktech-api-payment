using common.ApiSource.MercadoPago;
using common.DataSource;
using common.Enums;
using common.ExternalSource.MongoDb;
using driver.database.mongo.Entities;
using driver.database.mongo.Repositories;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Moq;
using unit_tests.helpers;

namespace unit_tests.databasemongo.Repositories
{
    public class PagamentoRepositoryTests
    {
        private readonly Mock<IMongoDatabase> mongoDatabaseMock;
        private readonly Mock<IMongoCollection<Pagamento>> mongoCollectionMock;
        private readonly Mock<IMongoClient> mongoClientMock;

        private readonly PagamentoRepository pagamentoRepository;
            
        public PagamentoRepositoryTests(){
            mongoDatabaseMock = new Mock<IMongoDatabase>();
            mongoCollectionMock = new Mock<IMongoCollection<Pagamento>>();
            mongoClientMock = new Mock<IMongoClient>();
            mongoCollectionMock.Setup(c => c.Indexes).Returns(new Mock<IMongoIndexManager<Pagamento>>().Object);
            mongoClientMock.Setup(c => c.GetDatabase("Test",null)).Returns(mongoDatabaseMock.Object);
            mongoDatabaseMock.Setup(d => d.GetCollection<Pagamento>("Pagamentos",null)).Returns(mongoCollectionMock.Object);
            pagamentoRepository = new PagamentoRepository(mongoDatabaseMock.Object);
        }

        [Fact]
        public async Task BuscarPagamentoPorPedidoIdRetornaPagamento()
        {
            
            Pagamento expectedPagamento = ObjectsBuilder.BuildPagamento();
            var pedidoId = expectedPagamento.PedidoId;
            var pagamentos = new List<Pagamento> { expectedPagamento };
            var mockCursor = new Mock<IAsyncCursor<Pagamento>>();
            
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                       .Returns(true)
                       .Returns(false);
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(true)
                       .ReturnsAsync(false);
            mockCursor.Setup(c => c.Current).Returns(pagamentos);

            mongoCollectionMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Pagamento>>(),
                                                   It.IsAny<FindOptions<Pagamento, Pagamento>>(),
                                                   It.IsAny<CancellationToken>()))
                           .ReturnsAsync(mockCursor.Object);

            var result = await pagamentoRepository.BuscarPagamentoPorPedidoId(pedidoId);

            Assert.NotNull(result);
            Assert.Equal(pedidoId, result.PedidoId);
        }

        [Fact]
        public async Task BuscarPagamentoPorPedidoIdRetornaNull()
        {
            
            var pedidoId = "123";
            var cursorMock = new Mock<IAsyncCursor<Pagamento>>();
            cursorMock.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
            cursorMock.SetupGet(c => c.Current).Returns(new List<Pagamento>());

            mongoCollectionMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Pagamento>>(),
                                                   It.IsAny<FindOptions<Pagamento, Pagamento>>(),
                                                   It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorMock.Object);

            
            var result = await pagamentoRepository.BuscarPagamentoPorPedidoId(pedidoId);

            
            Assert.Null(result);
        }

        [Fact]
        public async Task CriarPagamento_InsertsPagamento()
        {
            var pagamentoDto = new PagamentoEntityDto { pedidoDto = ObjectsBuilder.BuildPedidoDto(), pagamentoDto = ObjectsBuilder.BuildMercadoPagoQrCodeDto() };

            await pagamentoRepository.CriarPagamento(pagamentoDto);

            mongoCollectionMock.Verify(c => c.InsertOneAsync(It.Is<Pagamento>(p => p.PedidoId == pagamentoDto.pedidoDto.PedidoId.ToString()), null, default), Times.Once);
        }

        [Fact]
        public async Task AtualizarStatusPagamentoPorPedidoId_UpdatesStatus_WhenFound()
        {
            var pedidoId = "123";
            var novoStatus = StatusPagamento.Concluido;
            var dataAtualizacao = DateTime.UtcNow;
            var updateResult = new UpdateResult.Acknowledged(1, 1, null);

            mongoCollectionMock.Setup(c => c.UpdateOneAsync(It.IsAny<FilterDefinition<Pagamento>>(), It.IsAny<UpdateDefinition<Pagamento>>(), null, default))
                .ReturnsAsync(updateResult);

            var result = await pagamentoRepository.AtualizarStatusPagamentoPorPedidoId(pedidoId, novoStatus, dataAtualizacao);

            Assert.True(result);
        }

        [Fact]
        public async Task AtualizarStatusPagamentoPorPedidoId_ReturnsFalse_WhenNotFound()
        {
            var pedidoId = "123";
            var novoStatus = StatusPagamento.Concluido;
            var dataAtualizacao = DateTime.UtcNow;
            var updateResult = new UpdateResult.Acknowledged(0, 0, null);

            mongoCollectionMock.Setup(c => c.UpdateOneAsync(It.IsAny<FilterDefinition<Pagamento>>(), It.IsAny<UpdateDefinition<Pagamento>>(), null, default))
                .ReturnsAsync(updateResult);

            var result = await pagamentoRepository.AtualizarStatusPagamentoPorPedidoId(pedidoId, novoStatus, dataAtualizacao);

            Assert.False(result);
        }
    }
}