using common.DataSource;
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

        public Pedido(PedidoDto pedidoDto){
            var listaItens = new PedidoItem[]{new(pedidoDto.Itens)};
            Items = listaItens;
            TotalAmount = listaItens.Sum(l => l.TotalAmount);
            ExternalReference = pedidoDto.Id.ToString();
            Title = $"Pedido-{pedidoDto.Id}";
            Description = $"SnackTech-Pedido-{pedidoDto.Id}";
        }
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

        public PedidoItem(IEnumerable<PedidoItemDto> itensDoPedido){
            var valor = itensDoPedido.Sum(i => i.Produto.Valor * i.Quantidade);
            SkuNumber = "produto-001";
            Category = "Combo";
            Title = "Combo SnackTech";
            Description = "Conjunto de produtos da lanchonete SnackTeck";
            UnitPrice = valor;
            Quantity = 1;
            UnitMeasure = "unit";
            TotalAmount = valor;
        }
    }
}