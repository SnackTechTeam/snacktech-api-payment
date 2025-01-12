using Newtonsoft.Json;

namespace driver.mercado_pago.payloads
{
    public class AutenticacaoResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken {get; set;} = default!;

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType {get; set;} = default!;

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn {get; set;} = default!;

        [JsonProperty(PropertyName = "scope")]
        public string Scope {get; set;} = default!;

        [JsonProperty(PropertyName = "user_id")]
        public int UserId {get; set;} = default!;

        [JsonProperty(PropertyName = "live_mode")]
        public bool LiveMode {get; set;} = default!;
    }
}