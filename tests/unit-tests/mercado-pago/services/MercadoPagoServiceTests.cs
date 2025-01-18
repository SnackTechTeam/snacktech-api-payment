using System.Net;
using common.Interfaces;
using driver.mercado_pago.payloads;
using driver.mercado_pago.services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using unit_tests.helpers;

namespace unit_tests.mercadopago.services
{
    public class MercadoPagoServiceTests
    {
        private readonly IMercadoPagoIntegration mercadoPagoService;
        private readonly Mock<IHttpClientFactory> clientFactoryMock;

        private HttpClient ConfigureHttpClient(HttpStatusCode httpStatusCode,string content){
            var handlerMock = new Mock<HttpMessageHandler>();

             handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(content),
            });

            var httpClient = new HttpClient(handlerMock.Object);

            return httpClient;
        }
    
        public MercadoPagoServiceTests(){
            clientFactoryMock = new Mock<IHttpClientFactory>();
            mercadoPagoService = new MercadoPagoService(clientFactoryMock.Object);
        }


        [Fact]
        public async Task AutenticarComSucesso()
        {
           var response = new AutenticacaoResponse{
                AccessToken = "APP_USR-",
                TokenType = "Bearer",
                ExpiresIn = 21600,
                Scope = "read write",
                UserId = 2000007700,
                LiveMode = true
            };
            var responseSerialized = JsonConvert.SerializeObject(response);
            var httpClient = ConfigureHttpClient(HttpStatusCode.OK,responseSerialized);

            clientFactoryMock
                .Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var options = ObjectsBuilder.BuildMercadoPagoOptions();

            var autenticacao = await mercadoPagoService.Autenticar(options);
            Assert.NotNull(autenticacao);
            Assert.Equal(autenticacao.IdUsuario,response.UserId.ToString());
            Assert.Equal(autenticacao.TempoExpiracao, response.ExpiresIn);
            Assert.Equal(autenticacao.TokenDeAcesso, response.AccessToken);
        }

        [Fact]
        public async Task AutenticarComFalha(){
            var httpClient = ConfigureHttpClient(HttpStatusCode.BadRequest,"");
             clientFactoryMock
                .Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var options = ObjectsBuilder.BuildMercadoPagoOptions();

            try{
                await mercadoPagoService.Autenticar(options);
                Assert.Fail("Deveria lançar exception!");
            }
            catch(HttpRequestException hre){
                Assert.Equal("Response status code does not indicate success: 400 (Bad Request).",hre.Message);
            }
            catch(Exception e){
                Assert.Fail($"Lançou uma exception diferente da esperada. {e}");
            }

        }

        [Fact]
        public async Task GerarQrCodeComSucesso(){
            var options = ObjectsBuilder.BuildMercadoPagoOptions();
            var pedido = ObjectsBuilder.BuildPedidoDto();
            var accessToken = "accessToken";

            var response = new PedidoResponse{
                InStoreOrderId = "OrderId",
                QrData = "DadosQrCode"
            };
            var responseSerialized = JsonConvert.SerializeObject(response);
            var httpClient = ConfigureHttpClient(HttpStatusCode.OK,responseSerialized);
            clientFactoryMock
                .Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
            
            var mercadoPagoQrCodeDto = await mercadoPagoService.GerarQrCode(accessToken,options,pedido);

            Assert.Equal(response.QrData, mercadoPagoQrCodeDto.DadoDoCodigo);

        }

        [Fact]
        public async Task BuscarOrdemPagamentoSucesso(){
            var options = ObjectsBuilder.BuildMercadoPagoOptions();
            var accessToken = "accessToken";
            var orderId = "orderId";

            var reference = Guid.NewGuid();
            var response = new MerchantOrder{
                Id = 1000000,
                Status = "closed",
                ExternalReference = reference.ToString()
             };

            var responseSerialized = JsonConvert.SerializeObject(response);
            var httpClient = ConfigureHttpClient(HttpStatusCode.OK,responseSerialized);
            clientFactoryMock
                .Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var ordemPagamento = await mercadoPagoService.BuscarOrdemPagamento(accessToken,options,orderId);
            Assert.Equal(reference,ordemPagamento);
        }
    }
}