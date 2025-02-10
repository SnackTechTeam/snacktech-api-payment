using common.ApiSource.MercadoPago;
using common.DataSource;
using common.Interfaces;
using common.Options;
using core.gateways;
using Moq;
using unit_tests.helpers;

namespace unit_tests.core.gateways
{
    public class MercadoPagoGatewayTests
    {
        private readonly Mock<IMercadoPagoIntegration> _apiMercadoPagoMock;
        private readonly MercadoPagoOptions _mercadoPagoOptions;
        private readonly MercadoPagoGateway _mercadoPagoGateway;

        public MercadoPagoGatewayTests()
        {
            _apiMercadoPagoMock = new Mock<IMercadoPagoIntegration>();
            _mercadoPagoOptions = new MercadoPagoOptions
            {
                ClientId = "",
                ClientSecret = "",
                PosId = "",
                UrlBase = "",
                UserId = ""
            };
            _mercadoPagoGateway = new MercadoPagoGateway(_apiMercadoPagoMock.Object, _mercadoPagoOptions);
        }

        [Fact]
        public async Task IntegrarPedidoComSucesso()
        {
            // Arrange
            var pedidoDto = ObjectsBuilder.BuildPedidoDto();
            var autenticacao = new AutenticacaoMercadoPagoDto
            {
                TokenDeAcesso = "test_token"
            };
            var expectedQrCodeDto = new MercadoPagoQrCodeDto
            {
                // Initialize with necessary data
            };

            _apiMercadoPagoMock.Setup(api => api.Autenticar(_mercadoPagoOptions))
                .ReturnsAsync(autenticacao);
            _apiMercadoPagoMock.Setup(api => api.GerarQrCode(autenticacao.TokenDeAcesso, _mercadoPagoOptions, pedidoDto))
                .ReturnsAsync(expectedQrCodeDto);

            // Act
            var result = await _mercadoPagoGateway.IntegrarPedido(pedidoDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedQrCodeDto, result);
        }

        [Fact]
        public async Task BuscarPedidoViaOrderComSucesso()
        {
            // Arrange
            var orderId = "test_order_id";
            var autenticacao = new AutenticacaoMercadoPagoDto
            {
                TokenDeAcesso = "test_token"
            };
            var expectedGuid = Guid.NewGuid();

            _apiMercadoPagoMock.Setup(api => api.Autenticar(_mercadoPagoOptions))
                .ReturnsAsync(autenticacao);
            _apiMercadoPagoMock.Setup(api => api.BuscarOrdemPagamento(autenticacao.TokenDeAcesso, _mercadoPagoOptions, orderId))
                .ReturnsAsync(expectedGuid);

            // Act
            var result = await _mercadoPagoGateway.BuscarPedidoViaOrder(orderId);

            // Assert
            Assert.Equal(expectedGuid, result);
        }
    }
}