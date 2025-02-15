using System.Net;
using System.Text;
using behavior_tests.helpers;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Xunit;


namespace behavior_tests.steps
{
    [Binding]
    public class PagamentosControllerSteps
    {
        private readonly HttpClient client;
        private Mock<HttpMessageHandler> mockHttp;
        private HttpResponseMessage response;

        public PagamentosControllerSteps()
        {
            mockHttp = new Mock<HttpMessageHandler>();
            client = new HttpClient(mockHttp.Object){
                BaseAddress = new Uri("http://localhost:5000")
            };
            response = new HttpResponseMessage();
        }

        #region Background

        [Given(@"um pedido válido")]
        public void GivenQueEuTenhoPedidoValido(){

        }
        #endregion

        #region Cenário: Iniciar um pagamento
        [When(@"eu envio uma solicitação para criar um pagamento")]
        public async Task WhenEuEnvioSolicitacaoParaCriarPagamento(){
            mockHttp
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(new HttpResponseMessage{
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(StringContentBuilder.CriarPagamentoResponse())
                });

            var content = new StringContent(StringContentBuilder.CriarPagamentoBody(), 
                                            Encoding.UTF8,
                                            "application/json");

            response = await client.PostAsync("/api/pedidos", content);
        }

        [Then(@"eu devo receber um dado de pagamento válido")]
        public async Task ThenEuDevoReceberUmDadoDePagamentoValido(){
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            dynamic? resultado = JsonConvert.DeserializeObject(responseString);
            Assert.NotNull(resultado);
            Assert.NotNull(resultado?.id);
            Assert.NotNull(resultado?.qrCode);
            Assert.NotNull(resultado?.valorTotal);
        }
        #endregion

        #region Cenário: Finalizar um pagamento
        [Given(@"um pagamento retornado pela plataforma")]
        public void GivenUmPagamentoRetornadoPelaPlataforma(){

        }

        [When(@"eu envio um evento para indicar que o pagamento foi feito")]
        public async Task WhenEuEnvioEventoParaIndicarPagamentoFeito(){
            mockHttp
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(new HttpResponseMessage{
                    StatusCode = HttpStatusCode.OK
                });

            var content = new StringContent(StringContentBuilder.CriarFinalizacaoBody(), 
                                            Encoding.UTF8,
                                            "application/json");

            response = await client.PostAsync("/api/pedidos/finalizacao", content);
        }

        [Then(@"eu devo receber uma resposta de sucesso")]
        public void ThenEuDevoReceberRespostaDeSucesso(){
             response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        #endregion
    }
}