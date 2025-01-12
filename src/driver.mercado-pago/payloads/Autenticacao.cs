using Newtonsoft.Json;

namespace driver.mercado_pago.payloads
{
    public class Autenticacao(string clientId, string clientSecret)
    {
        [JsonProperty(PropertyName = "client_secret")]
        public string ClientSecret { get; set; } = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));

        [JsonProperty(PropertyName = "client_id")]
        public string ClientId { get; set; } = clientId ?? throw new ArgumentNullException(nameof(clientId));

        [JsonProperty(PropertyName = "grant_type")]
        public string GrantType { get; set; } = "client_credentials";

        [JsonProperty(PropertyName = "test_token")]
        public string TestToken { get; set; } = "false";

        public static Autenticacao CriarPayload(string clientId, string clientSecret)
            => new(clientId,clientSecret);
    }
}