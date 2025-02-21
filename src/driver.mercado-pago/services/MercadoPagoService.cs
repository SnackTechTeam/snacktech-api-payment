using common.ApiSource.MercadoPago;
using common.DataSource;
using common.Interfaces;
using common.Options;
using driver.mercado_pago.payloads;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace driver.mercado_pago.services
{
    public class MercadoPagoService(IHttpClientFactory httpClientFactory, ILogger<MercadoPagoService> logger) : IMercadoPagoIntegration
    {
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;   
        private readonly ILogger<MercadoPagoService> logger = logger;

        public async Task<AutenticacaoMercadoPagoDto> Autenticar(MercadoPagoOptions mercadoPagoOptions){
            var rota = "oauth/token";

            var objetoPayload = Autenticacao.CriarPayload(mercadoPagoOptions.ClientId,mercadoPagoOptions.ClientSecret);

            var conteudoJson = GerarConteudoParaRequisicao(objetoPayload);

            var clientHttp = CriarHttpClientBase(mercadoPagoOptions.UrlBase);
            logger.LogInformation($"Iniciando autenticação com o mercado pago. URL {mercadoPagoOptions.UrlBase}/{rota}. ClientId: {mercadoPagoOptions.ClientId}");
            var resposta = await clientHttp.PostAsync(rota,conteudoJson);
            logger.LogInformation($"Resposta da autenticação com o mercado pago. Status: {resposta.StatusCode}. {resposta.ReasonPhrase}");
            resposta.EnsureSuccessStatusCode();

            var conteudoRespostaRequisicao = await RetornarConteudo<AutenticacaoResponse>(resposta);

            AutenticacaoMercadoPagoDto dadosAutenticacao = conteudoRespostaRequisicao;

            return dadosAutenticacao;
        }

        public async Task<MercadoPagoQrCodeDto> GerarQrCode(string accessToken, MercadoPagoOptions mercadoPagoOptions, PedidoDto pedido){
            var userId = mercadoPagoOptions.UserId;
            var posId = mercadoPagoOptions.PosId;
            var serviceUrl = mercadoPagoOptions.UrlBase;

            var pedidoMercadoPago = new Pedido(pedido);
            var rota = $"instore/orders/qr/seller/collectors/{userId}/pos/{posId}/qrs";

            var content = GerarConteudoParaRequisicao(pedidoMercadoPago);
            var httpClient = CriarHttpClientBase(serviceUrl);
            httpClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {accessToken}");

            logger.LogInformation($"Gerando QR Code para o pedido {pedidoMercadoPago.ExternalReference}. URL {serviceUrl}/{rota}");
            var resposta = await httpClient.PutAsync(rota, content);
            logger.LogInformation($"Resposta da geração do QR Code para o pedido {pedidoMercadoPago.ExternalReference}. Status: {resposta.StatusCode}. {resposta.ReasonPhrase}");
            var conteudoResposta = await RetornarConteudo<PedidoResponse>(resposta);

            MercadoPagoQrCodeDto retorno = PedidoResponse.ConverterParaMercadoPagoQrCodeDto(conteudoResposta,pedidoMercadoPago.TotalAmount);

            return retorno;
        }

        public async Task<Guid> BuscarOrdemPagamento(string accessToken, MercadoPagoOptions mercadoPagoOptions, string orderId){
             var rota = $"merchant_orders/{orderId}";

            var httpClient = CriarHttpClientBase(mercadoPagoOptions.UrlBase);
            httpClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {accessToken}");

            logger.LogInformation($"Buscando ordem de pagamento com o orderId {orderId}. URL {mercadoPagoOptions.UrlBase}/{rota}");
            var resposta = await httpClient.GetAsync(rota);
            logger.LogInformation($"Resposta da busca da ordem de pagamento com o orderId {orderId}. Status: {resposta.StatusCode}. {resposta.ReasonPhrase}");
            var conteudoResposta = await RetornarConteudo<MerchantOrder>(resposta);

            return Guid.Parse(conteudoResposta.ExternalReference);
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