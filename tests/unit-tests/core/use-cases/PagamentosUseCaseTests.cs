using common.Api;
using common.ApiSource.MercadoPago;
using common.DataSource;
using common.Enums;
using common.ExternalSource.MongoDb;
using common.ExternalSource.Sqs;
using core.domain;
using core.interfaces.gateways;
using core.usecases;
using Moq;
using unit_tests.helpers;

namespace unittests.core.usecases
{
    public class PagamentosUseCaseTests
    {
        
        private readonly Mock<IMercadoPagoGateway> mercadoPagoGatewayMock;
        private readonly Mock<IMongoDbGateway> mongoDbGatewayMock;
        private readonly Mock<ISqsGateway> sqsGatewayMock;
        private readonly PedidoDto pedidoMock;

        public PagamentosUseCaseTests()
        {
            mercadoPagoGatewayMock = new Mock<IMercadoPagoGateway>();
            mongoDbGatewayMock = new Mock<IMongoDbGateway>();
            sqsGatewayMock = new Mock<ISqsGateway>();
            pedidoMock  = ObjectsBuilder.BuildPedidoDto();
        }

        [Fact]
        public async Task GerarPagamentoAtravesDePedidoDeveRetornarPagamento()
        {
            
            var qrCodeDto = new MercadoPagoQrCodeDto();
            
            mercadoPagoGatewayMock
                .Setup(x => x.IntegrarPedido(pedidoMock))
                .ReturnsAsync(qrCodeDto);

            mongoDbGatewayMock
                .Setup(x => x.GravarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>()))
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await PagamentosUseCase.GerarPagamentoAtravesDePedido(
                mercadoPagoGatewayMock.Object,
                mongoDbGatewayMock.Object,
                pedidoMock);

            // Assert
            Assert.True(resultado.Sucesso);
            mongoDbGatewayMock.Verify(x => x.GravarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>()), Times.Once);
        }

        [Fact]
        public async Task GerarPagamentoAtravesDePedidoQuandoMercadoPagoFalhaDeveRetornarErro()
        {

            mercadoPagoGatewayMock
                .Setup(x => x.IntegrarPedido(pedidoMock))
                .ThrowsAsync(new Exception("Erro na integração"));


            var resultado = await PagamentosUseCase.GerarPagamentoAtravesDePedido(
                mercadoPagoGatewayMock.Object,
                mongoDbGatewayMock.Object,
                pedidoMock);

            Assert.False(resultado.Sucesso);
            mongoDbGatewayMock.Verify(x => x.GravarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>()), Times.Never);
        }

        [Fact]
        public async Task GerarPagamentoAtravesDePedidoQuandoMongoDbFalhaDeveRetornarErro()
        {
            var qrCodeDto = new MercadoPagoQrCodeDto();

            mercadoPagoGatewayMock
                .Setup(x => x.IntegrarPedido(pedidoMock))
                .ReturnsAsync(qrCodeDto);

            mongoDbGatewayMock
                .Setup(x => x.GravarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>()))
                .ThrowsAsync(new Exception("Erro ao salvar"));

            var resultado = await PagamentosUseCase.GerarPagamentoAtravesDePedido(
                mercadoPagoGatewayMock.Object,
                mongoDbGatewayMock.Object,
                pedidoMock);

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public async Task ProcessarPagamentoRealizadoQuandoAcaoCreateDeveRetornarSucesso()
        {
            // Arrange
            var pagamento = new PagamentoProcessadoDto { Action = "create" };

            // Act
            var resultado = await PagamentosUseCase.ProcessarPagamentoRealizado(
                mercadoPagoGatewayMock.Object,
                mongoDbGatewayMock.Object,
                sqsGatewayMock.Object,
                pagamento);

            // Assert
            Assert.True(resultado.Sucesso);
            mercadoPagoGatewayMock.Verify(x => x.BuscarPedidoViaOrder(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ProcessarPagamentoRealizadoQuandoStatusNaoClosedDeveRetornarSucesso()
        {
            // Arrange
            var pagamento = new PagamentoProcessadoDto { 
                Action = "payment.updated",
                Status = "pending"
            };

            // Act
            var resultado = await PagamentosUseCase.ProcessarPagamentoRealizado(
                mercadoPagoGatewayMock.Object,
                mongoDbGatewayMock.Object,
                sqsGatewayMock.Object,
                pagamento);

            // Assert
            Assert.True(resultado.Sucesso);
            mercadoPagoGatewayMock.Verify(x => x.BuscarPedidoViaOrder(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ProcessarPagamentoRealizadoQuandoPagamentoConcluidoDeveProcessarComSucesso()
        {
            // Arrange
            var pagamentoId = "123";
            var pedidoId = Guid.NewGuid();
            var pagamento = new PagamentoProcessadoDto { 
                Id = pagamentoId,
                Action = "payment.updated",
                Status = "closed"
            };

            var buscaPagamentoDto = new BuscaPagamentoDto{
                ClienteId = Guid.NewGuid().ToString(),
                DataCriacao = DateTime.Now,
                DataUltimaAtualizacao = null,
                PagamentoId = pagamentoId,
                PedidoId = pedidoId.ToString(),
                Status = StatusPagamento.Pendente.ToString(),
                Valor = 10.5M
            };

            mercadoPagoGatewayMock
                .Setup(x => x.BuscarPedidoViaOrder(pagamentoId))
                .ReturnsAsync(pedidoId);

            mongoDbGatewayMock
                .Setup(x => x.BuscarPagamentoPorPedidoId(pedidoId))
                .ReturnsAsync(buscaPagamentoDto);

            mongoDbGatewayMock
                .Setup(x => x.AtualizarPagamentoPorPedidoId(pedidoId, StatusPagamento.Concluido, It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await PagamentosUseCase.ProcessarPagamentoRealizado(
                mercadoPagoGatewayMock.Object,
                mongoDbGatewayMock.Object,
                sqsGatewayMock.Object,
                pagamento);

            // Assert
            Assert.True(resultado.Sucesso);
            sqsGatewayMock.Verify(x => x.PublicarMensagemPagamentoNoSqs(It.IsAny<PagamentoMessageDto>()), Times.Once);
        }

        [Fact]
        public async Task ProcessarPagamentoRealizadoQuandoPagamentoNaoEncontradoDeveRetornarErro()
        {
            // Arrange
            var pagamentoId = "123";
            var pedidoId = Guid.NewGuid();
            var pagamento = new PagamentoProcessadoDto { 
                Id = pagamentoId,
                Action = "payment.updated",
                Status = "closed"
            };

            mercadoPagoGatewayMock
                .Setup(x => x.BuscarPedidoViaOrder(pagamentoId))
                .ReturnsAsync(pedidoId);

            mongoDbGatewayMock
                .Setup(x => x.BuscarPagamentoPorPedidoId(pedidoId))
                .ReturnsAsync((BuscaPagamentoDto)null);

            // Act
            var resultado = await PagamentosUseCase.ProcessarPagamentoRealizado(
                mercadoPagoGatewayMock.Object,
                mongoDbGatewayMock.Object,
                sqsGatewayMock.Object,
                pagamento);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains($"Não foi encontrado pagamento gravado para pedido {pedidoId}", resultado.Mensagem);
        }

        [Fact]
        public async Task ProcessarPagamentoRealizadoQuandoAtualizacaoFalhaDeveRetornarErro()
        {
            // Arrange
            var pagamentoId = "123";
            var pedidoId = Guid.NewGuid();
            var pagamento = new PagamentoProcessadoDto { 
                Id = pagamentoId,
                Action = "payment.updated",
                Status = "closed"
            };

            var buscaPagamentoDto = new BuscaPagamentoDto();

            mercadoPagoGatewayMock
                .Setup(x => x.BuscarPedidoViaOrder(pagamentoId))
                .ReturnsAsync(pedidoId);

            mongoDbGatewayMock
                .Setup(x => x.BuscarPagamentoPorPedidoId(pedidoId))
                .ReturnsAsync(buscaPagamentoDto);

            mongoDbGatewayMock
                .Setup(x => x.AtualizarPagamentoPorPedidoId(pedidoId, StatusPagamento.Concluido, It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await PagamentosUseCase.ProcessarPagamentoRealizado(
                mercadoPagoGatewayMock.Object,
                mongoDbGatewayMock.Object,
                sqsGatewayMock.Object,
                pagamento);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains($"Não foi possível atualizar pagamento do pedido {pedidoId}", resultado.Mensagem);
        }

        [Fact]
        public async Task ProcessarPagamentoViaMockQuandoSucessoDeveRetornarSucesso()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();

            var buscaPagamentoDto = new BuscaPagamentoDto{
                ClienteId = Guid.NewGuid().ToString(),
                DataCriacao = DateTime.Now,
                DataUltimaAtualizacao = null,
                PagamentoId = "123",
                PedidoId = pedidoId.ToString(),
                Status = StatusPagamento.Pendente.ToString(),
                Valor = 10.5M
            };

            mongoDbGatewayMock.Setup(x => x.BuscarPagamentoPorPedidoId(pedidoId))
                .ReturnsAsync(buscaPagamentoDto);
            
            mongoDbGatewayMock.Setup(x => x.AtualizarPagamentoPorPedidoId(
                It.IsAny<Guid>(), 
                It.IsAny<StatusPagamento>(), 
                It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await PagamentosUseCase.ProcessarPagamentoViaMock(
                mongoDbGatewayMock.Object, 
                sqsGatewayMock.Object, 
                pedidoId);

            // Assert
            Assert.True(resultado.Sucesso);
            mongoDbGatewayMock.Verify(x => x.AtualizarPagamentoPorPedidoId(
                pedidoId, 
                StatusPagamento.Concluido, 
                It.IsAny<DateTime>()), 
                Times.Once);
            sqsGatewayMock.Verify(x => x.PublicarMensagemPagamentoNoSqs(
                It.IsAny<PagamentoMessageDto>()), 
                Times.Once);
        }

        [Fact]
        public async Task ProcessarPagamentoViaMockQuandoPagamentoNaoEncontradoDeveRetornarErro()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            mongoDbGatewayMock.Setup(x => x.BuscarPagamentoPorPedidoId(pedidoId))
                .ReturnsAsync((BuscaPagamentoDto)null);

            // Act
            var resultado = await PagamentosUseCase.ProcessarPagamentoViaMock(
                mongoDbGatewayMock.Object, 
                sqsGatewayMock.Object, 
                pedidoId);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains($"Não foi encontrado pagamento gravado para pedido {pedidoId}", 
                resultado.Mensagem);
            mongoDbGatewayMock.Verify(x => x.AtualizarPagamentoPorPedidoId(
                It.IsAny<Guid>(), 
                It.IsAny<StatusPagamento>(), 
                It.IsAny<DateTime>()), 
                Times.Never);
        }

        [Fact]
        public async Task ProcessarPagamentoViaMockQuandoAtualizacaoFalhaDeveRetornarErro()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var buscaPagamentoDto = new BuscaPagamentoDto();

            mongoDbGatewayMock.Setup(x => x.BuscarPagamentoPorPedidoId(pedidoId))
                .ReturnsAsync(buscaPagamentoDto);
            
            mongoDbGatewayMock.Setup(x => x.AtualizarPagamentoPorPedidoId(
                It.IsAny<Guid>(), 
                It.IsAny<StatusPagamento>(), 
                It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await PagamentosUseCase.ProcessarPagamentoViaMock(
                mongoDbGatewayMock.Object, 
                sqsGatewayMock.Object, 
                pedidoId);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains($"Não foi possível atualizar pagamento do pedido {pedidoId}", 
                resultado.Mensagem);
            sqsGatewayMock.Verify(x => x.PublicarMensagemPagamentoNoSqs(
                It.IsAny<PagamentoMessageDto>()), 
                Times.Never);
        }

        [Fact]
        public async Task ProcessarPagamentoViaMockQuandoOcorreArgumentExceptionDeveRetornarErroLogico()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var mensagemErro = "Erro de argumento";
            mongoDbGatewayMock.Setup(x => x.BuscarPagamentoPorPedidoId(pedidoId))
                .ThrowsAsync(new ArgumentException(mensagemErro));

            // Act
            var resultado = await PagamentosUseCase.ProcessarPagamentoViaMock(
                mongoDbGatewayMock.Object, 
                sqsGatewayMock.Object, 
                pedidoId);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(mensagemErro, resultado.Mensagem);
        }

        [Fact]
        public async Task ProcessarPagamentoViaMockQuandoOcorreExceptionDeveRetornarErroInterno()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            mongoDbGatewayMock.Setup(x => x.BuscarPagamentoPorPedidoId(pedidoId))
                .ThrowsAsync(new Exception("Erro interno na aplicação"));

            // Act
            var resultado = await PagamentosUseCase.ProcessarPagamentoViaMock(
                mongoDbGatewayMock.Object, 
                sqsGatewayMock.Object, 
                pedidoId);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal("Erro interno na aplicação", resultado.Mensagem);
        }

        [Fact]
        public async Task GerarPagamentoViaMockComSucesso()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var sqsGatewayMock = new Mock<ISqsGateway>();
            var buscaPagamentoDto = new BuscaPagamentoDto{
                ClienteId = Guid.NewGuid().ToString(),
                DataCriacao = DateTime.Now,
                DataUltimaAtualizacao = null,
                PagamentoId = "123",
                PedidoId = pedidoId.ToString(),
                Status = StatusPagamento.Pendente.ToString(),
                Valor = 10.5M
            };

            mongoDbGatewayMock.Setup(m => m.GravarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>())).Returns(Task.CompletedTask);
            mongoDbGatewayMock.Setup(m => m.BuscarPagamentoPorPedidoId(It.IsAny<Guid>())).ReturnsAsync(buscaPagamentoDto);
            sqsGatewayMock.Setup(s => s.PublicarMensagemPagamentoNoSqs(It.IsAny<PagamentoMessageDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await PagamentosUseCase.GerarPagamentoViaMock(mongoDbGatewayMock.Object, sqsGatewayMock.Object, pedidoMock);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Sucesso);
            mongoDbGatewayMock.Verify(m => m.GravarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>()), Times.Once);
            mongoDbGatewayMock.Verify(m => m.BuscarPagamentoPorPedidoId(It.IsAny<Guid>()), Times.Once);
            sqsGatewayMock.Verify(s => s.PublicarMensagemPagamentoNoSqs(It.IsAny<PagamentoMessageDto>()), Times.Once);
        }

        [Fact]
        public async Task GerarPagamentoViaMockPagamentoNaoEncontrado()
        {
            // Arrange
            mongoDbGatewayMock.Setup(m => m.GravarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>())).Returns(Task.CompletedTask);
            mongoDbGatewayMock.Setup(m => m.BuscarPagamentoPorPedidoId(It.IsAny<Guid>())).ReturnsAsync((BuscaPagamentoDto)null);

            // Act
            var result = await PagamentosUseCase.GerarPagamentoViaMock(mongoDbGatewayMock.Object, sqsGatewayMock.Object, pedidoMock);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Equal("Pagamento não encontrado na base de dados.", result.Mensagem);
            mongoDbGatewayMock.Verify(m => m.GravarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>()), Times.Once);
            mongoDbGatewayMock.Verify(m => m.BuscarPagamentoPorPedidoId(It.IsAny<Guid>()), Times.Once);
            sqsGatewayMock.Verify(s => s.PublicarMensagemPagamentoNoSqs(It.IsAny<PagamentoMessageDto>()), Times.Never);
        }

        [Fact]
        public async Task GerarPagamentoViaMockTratandoException()
        {
            // Arrange
          
            mongoDbGatewayMock.Setup(m => m.GravarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>())).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await PagamentosUseCase.GerarPagamentoViaMock(mongoDbGatewayMock.Object, sqsGatewayMock.Object, pedidoMock);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Sucesso);
            Assert.Equal("Database error", result.Mensagem);
            mongoDbGatewayMock.Verify(m => m.GravarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>()), Times.Once);
            mongoDbGatewayMock.Verify(m => m.BuscarPagamentoPorPedidoId(It.IsAny<Guid>()), Times.Never);
            sqsGatewayMock.Verify(s => s.PublicarMensagemPagamentoNoSqs(It.IsAny<PagamentoMessageDto>()), Times.Never);
        }
    }
}