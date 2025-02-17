
using common.Api;
using common.ApiSource.MercadoPago;
using common.DataSource;
using common.Enums;
using common.ExternalSource.MongoDb;
using common.Interfaces;
using common.Options;
using core.controllers;
using Microsoft.Extensions.Options;
using Moq;
using unit_tests.helpers;

namespace unit_tests.core.controllers
{
    public class PagamentoControllerTests
    {
        private readonly Mock<IMercadoPagoIntegration> mercadoPagoIntegrationMock;
        private readonly Mock<IMongoDbIntegration> mongoDbIntegrationMock;
        private readonly Mock<ISqsIntegration> sqsIntegrationMock;
        private readonly Mock<IOptions<MercadoPagoOptions>> mercadoPagoOptionsMock;
        private readonly PagamentoController controller;

        public PagamentoControllerTests()
        {
            mercadoPagoIntegrationMock = new Mock<IMercadoPagoIntegration>();
            mongoDbIntegrationMock = new Mock<IMongoDbIntegration>();
            sqsIntegrationMock = new Mock<ISqsIntegration>();
            mercadoPagoOptionsMock = new Mock<IOptions<MercadoPagoOptions>>();
            mercadoPagoOptionsMock.Setup(o => o.Value).Returns(new MercadoPagoOptions());

            controller = new PagamentoController(
                mercadoPagoIntegrationMock.Object,
                mongoDbIntegrationMock.Object,
                sqsIntegrationMock.Object,
                mercadoPagoOptionsMock.Object
            );
        }

        [Fact]
        public async Task CriarPagamentoComSucesso(){
            mercadoPagoIntegrationMock.Setup(m => m.Autenticar(It.IsAny<MercadoPagoOptions>())).ReturnsAsync(new AutenticacaoMercadoPagoDto());
            mercadoPagoIntegrationMock.Setup(m => m.GerarQrCode(It.IsAny<string>(), It.IsAny<MercadoPagoOptions>(), It.IsAny<PedidoDto>())).ReturnsAsync(new MercadoPagoQrCodeDto());
            mongoDbIntegrationMock.Setup(m => m.CriarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>()));

            var resultado = await controller.CriarPagamento(ObjectsBuilder.BuildPedidoDto());

            Assert.True(resultado.Sucesso); 
            Assert.NotNull(resultado.Dados);
        }

        [Fact]
        public async Task CriarPagamentoComFalha(){
             mercadoPagoIntegrationMock.Setup(m => m.Autenticar(It.IsAny<MercadoPagoOptions>())).ThrowsAsync(new Exception("Erro inesperado"));

             var resultado = await controller.CriarPagamento(ObjectsBuilder.BuildPedidoDto());

             Assert.False(resultado.Sucesso);
             Assert.NotNull(resultado.Excecao);
        }

        [Fact]
        public async Task CriarPagamentoMockComSucesso(){

            var buscaPagamento = new BuscaPagamentoDto(){PagamentoId = Guid.NewGuid().ToString()};
            mongoDbIntegrationMock.Setup(m => m.CriarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>()));
            mongoDbIntegrationMock.Setup(m => m.BuscarPagamentoPorPedidoId(It.IsAny<string>())).ReturnsAsync(buscaPagamento);
            sqsIntegrationMock.Setup(m => m.SendMessageAsync(It.IsAny<Type>()));

            var resultado = await controller.CriarPagamentoMock(ObjectsBuilder.BuildPedidoDto());

            Assert.True(resultado.Sucesso, resultado.Mensagem); 
            Assert.NotNull(resultado.Dados);
        }

        [Fact]
        public async Task CriarPagamentoMockComFalha(){
            mongoDbIntegrationMock.Setup(m => m.CriarPagamento(It.IsAny<PagamentoEntityDto>(), It.IsAny<StatusPagamento>())).ThrowsAsync(new Exception("Erro inesperado"));
            var resultado = await controller.CriarPagamentoMock(ObjectsBuilder.BuildPedidoDto());
             Assert.False(resultado.Sucesso);
             Assert.NotNull(resultado.Excecao);
        }

        [Fact]
        public async Task ProcessarPagamentoComSucesso(){
            var pagamentoProcessado = ObjectsBuilder.BuildPagamentoProcessadoDto();
            var buscaPagamento = new BuscaPagamentoDto(){PagamentoId = Guid.NewGuid().ToString()};
            mercadoPagoIntegrationMock.Setup(m => m.Autenticar(It.IsAny<MercadoPagoOptions>())).ReturnsAsync(new AutenticacaoMercadoPagoDto());
            mercadoPagoIntegrationMock.Setup(m => m.BuscarOrdemPagamento(It.IsAny<string>(), It.IsAny<MercadoPagoOptions>(), It.IsAny<string>())).ReturnsAsync(Guid.NewGuid());
            mongoDbIntegrationMock.Setup(m => m.BuscarPagamentoPorPedidoId(It.IsAny<string>())).ReturnsAsync(buscaPagamento);
            mongoDbIntegrationMock.Setup(m => m.AtualizarStatusPagamentoPorPedidoId(It.IsAny<string>(), It.IsAny<StatusPagamento>(), It.IsAny<DateTime>())).ReturnsAsync(true);
            sqsIntegrationMock.Setup(m => m.SendMessageAsync(It.IsAny<Type>()));

            var resultado = await controller.ProcessarPagamento(pagamentoProcessado);

            Assert.True(resultado.Sucesso); 
        }

        [Fact]
        public async Task ProcessarPagamentoComFalha(){
            var pagamentoProcessado = ObjectsBuilder.BuildPagamentoProcessadoDto();
            mercadoPagoIntegrationMock.Setup(m => m.Autenticar(It.IsAny<MercadoPagoOptions>())).ThrowsAsync(new Exception("Erro inesperado"));

            var resultado = await controller.ProcessarPagamento(pagamentoProcessado);

            Assert.False(resultado.Sucesso);
            Assert.NotNull(resultado.Excecao);
        }

        [Fact]
        public async Task ProcessarPagamentoMockComSucesso(){
            var identificacaoPedido = Guid.NewGuid();
            var buscaPagamento = new BuscaPagamentoDto(){PagamentoId = Guid.NewGuid().ToString()};
            mongoDbIntegrationMock.Setup(m => m.BuscarPagamentoPorPedidoId(It.IsAny<string>())).ReturnsAsync(buscaPagamento);
            mongoDbIntegrationMock.Setup(m => m.AtualizarStatusPagamentoPorPedidoId(It.IsAny<string>(), It.IsAny<StatusPagamento>(), It.IsAny<DateTime>())).ReturnsAsync(true);
            sqsIntegrationMock.Setup(m => m.SendMessageAsync(It.IsAny<Type>()));

            var resultado = await controller.ProcessarPagamentoMock(identificacaoPedido);

            Assert.True(resultado.Sucesso); 
        }

        [Fact]
        public async Task ProcesarPagamentoMockComFalha(){
            mongoDbIntegrationMock.Setup(m => m.BuscarPagamentoPorPedidoId(It.IsAny<string>())).ThrowsAsync(new Exception("Erro inesperado"));

            var resultado = await controller.ProcessarPagamentoMock(Guid.NewGuid());

            Assert.False(resultado.Sucesso);
            Assert.NotNull(resultado.Excecao);
        }
    }
}