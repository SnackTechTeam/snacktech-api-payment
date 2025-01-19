using Newtonsoft.Json;

namespace common.Api
{
    public class PagamentoProcessadoDto
    {
        [JsonProperty(PropertyName = "action")]
        public string Action {get; set;} = default!;

        [JsonProperty(PropertyName = "application_id")]
        public string ApplicationId {get; set;} = default!;

        [JsonProperty(PropertyName = "data")]
        public PagamentoProcessadoDataDto Data {get; set;} = default!;

        [JsonProperty(PropertyName = "date_created")]
        public string DateCreated {get; set;} = default!;

        [JsonProperty(PropertyName = "id")]
        public string Id {get; set;} = default!;

        [JsonProperty(PropertyName = "live_mode")]
        public bool LiveMode {get; set;}

        [JsonProperty(PropertyName = "status")]
        public string Status {get; set;} = default!;

        [JsonProperty(PropertyName = "type")]
        public string Type {get; set;} = default!;

        [JsonProperty(PropertyName = "user_id")]
        public long UserId {get; set;} = default!;

        [JsonProperty(PropertyName = "version")]
        public int Version {get; set;}
    }

    public class PagamentoProcessadoDataDto{
        [JsonProperty(PropertyName = "currency_id")]
        public string CurrencyId {get; set;} = default!;

        [JsonProperty(PropertyName = "marketplace")]
        public string Marketplace {get; set;} = default!;

        [JsonProperty(PropertyName = "status")]
        public string Status {get; set;} = default!;
    }
}