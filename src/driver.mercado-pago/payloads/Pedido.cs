using Newtonsoft.Json;

namespace driver.mercado_pago.payloads
{
    public class Pedido
    {
        [JsonProperty(PropertyName = "external_reference")]
        public string ExternalReference {get; set;}

        [JsonProperty(PropertyName = "title")]
        public string Title {get; set;}

        [JsonProperty(PropertyName = "description")]
        public string Description {get; set;}

        [JsonProperty(PropertyName = "total_amount")]
        public decimal TotalAmount {get; set;}

        [JsonProperty(PropertyName = "items")]
        public IEnumerable<PedidoItem> Items {get; set;}
    }

    public class PedidoItem{
        [JsonProperty(PropertyName = "sku_number")]
        public string SkuNumber {get; set;}

        [JsonProperty(PropertyName = "category")]
        public string Category {get; set;}

        [JsonProperty(PropertyName = "title")]
        public string Title {get; set;}

        [JsonProperty(PropertyName = "description")]
        public string Description {get; set;}

        [JsonProperty(PropertyName = "unit_price")]
        public decimal UnitPrice {get; set;}

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity {get; set;}

        [JsonProperty(PropertyName = "unit_measure")]
        public string UnitMeasure {get; set;}

        [JsonProperty(PropertyName = "total_amount")]
        public decimal TotalAmount {get; set;}
    }
}