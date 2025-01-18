using Newtonsoft.Json;

namespace driver.mercado_pago.payloads
{
    public class MerchantOrder
    {
        /*
            No retorno da chamada ao MP existem mais dados.
            Mas os descritos abaixo atendem nosso escopo
        */
        [JsonProperty(PropertyName = "id")]
        public long Id {get ;set;}

        [JsonProperty(PropertyName = "status")]
        public required string Status {get; set;}

        [JsonProperty(PropertyName = "external_reference")]
        public required string ExternalReference {get; set;}
    }
}