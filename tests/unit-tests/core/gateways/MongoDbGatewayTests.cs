using common.Enums;
using common.ExternalSource.MongoDb;
using common.Interfaces;
using core.gateways;
using Moq;

namespace unit_tests.core.gateways
{
    public class MongoDbGatewayTests
    {
        private readonly Mock<IMongoDbIntegration> _mongoDbIntegrationMock;
        private readonly MongoDbGateway _mongoDbGateway;

        public MongoDbGatewayTests()
        {
            _mongoDbIntegrationMock = new Mock<IMongoDbIntegration>();
            _mongoDbGateway = new MongoDbGateway(_mongoDbIntegrationMock.Object);
        }

        [Fact]
        public async Task GravarPagamentoComSucesso()
        {
            // Arrange
            var pagamentoEntityDto = new PagamentoEntityDto();

            _mongoDbIntegrationMock.Setup(m => m.CriarPagamento(pagamentoEntityDto, It.IsAny<StatusPagamento>()))
                .Returns(Task.CompletedTask);

            // Act
            await _mongoDbGateway.GravarPagamento(pagamentoEntityDto, StatusPagamento.Pendente);

            // Assert
            _mongoDbIntegrationMock.Verify(m => m.CriarPagamento(pagamentoEntityDto, It.IsAny<StatusPagamento>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarPagamentoPorPedidoIdComSucesso()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var statusPagamento = StatusPagamento.Concluido;
            var dataAtualizacao = DateTime.UtcNow;

            _mongoDbIntegrationMock.Setup(m => m.AtualizarStatusPagamentoPorPedidoId(pedidoId.ToString(), statusPagamento, dataAtualizacao))
                .ReturnsAsync(true);

            // Act
            var result = await _mongoDbGateway.AtualizarPagamentoPorPedidoId(pedidoId, statusPagamento, dataAtualizacao);

            // Assert
            Assert.True(result);
            _mongoDbIntegrationMock.Verify(m => m.AtualizarStatusPagamentoPorPedidoId(pedidoId.ToString(), statusPagamento, dataAtualizacao), Times.Once);
        }

        [Fact]
        public async Task BuscarPagamentoPorPedidoIdComSucesso()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var expectedBuscaPagamentoDto = new BuscaPagamentoDto();

            _mongoDbIntegrationMock.Setup(m => m.BuscarPagamentoPorPedidoId(pedidoId.ToString()))
                .ReturnsAsync(expectedBuscaPagamentoDto);

            // Act
            var result = await _mongoDbGateway.BuscarPagamentoPorPedidoId(pedidoId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBuscaPagamentoDto, result);
            _mongoDbIntegrationMock.Verify(m => m.BuscarPagamentoPorPedidoId(pedidoId.ToString()), Times.Once);
        }
    }
}