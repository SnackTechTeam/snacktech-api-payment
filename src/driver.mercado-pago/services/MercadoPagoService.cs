using driver.mercado_pago.payloads;
using Newtonsoft.Json;

namespace driver.mercado_pago.services
{
    public class MercadoPagoService(IHttpClientFactory httpClientFactory)
    {
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;   

        //TODO: Passar parametro com valores de autenticacao
        public async Task Autenticar(){
            var rota = "oauth/token";

            //TODO: Preencher parametros correto
            var objetoPayload = Autenticacao.CriarPayload("","");

            var conteudoJson = GerarConteudoParaRequisicao(objetoPayload);

            //TODO: Preencher parametro correto
            var clientHttp = CriarHttpClientBase("");

            var resposta = await clientHttp.PostAsync(rota,conteudoJson);

            resposta.EnsureSuccessStatusCode();

            var conteudoRespostaRequisicao = await RetornarConteudo<AutenticacaoResponse>(resposta);

            conteudoRespostaRequisicao.AccessToken = "";

            //TODO: Retornar DTO
        }

        private HttpClient CriarHttpClientBase(string urlBase){
            var httpClient = httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(urlBase);

            return httpClient;
        }

        private static async Task<T> RetornarConteudo<T>(HttpResponseMessage resposta){
            resposta.EnsureSuccessStatusCode();

            var content = await resposta.Content.ReadAsStringAsync();
            var objeto = JsonConvert.DeserializeObject<T>(content);
            return objeto ?? throw new InvalidOperationException("Objeto do conteudo está nulo.");
        }

        private static StringContent GerarConteudoParaRequisicao<T>(T objetoConteudo){
            var objetoSerializado = JsonConvert.SerializeObject(objetoConteudo);
            var content = new StringContent(objetoSerializado);
            return content;
        }
    }
}