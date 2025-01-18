using common.ApiSource.MercadoPago;
using Newtonsoft.Json;

namespace driver.mercado_pago.payloads
{
    public class PedidoResponse
    {
        [JsonProperty(PropertyName = "in_store_order_id")]
        public string InStoreOrderId {get; set;} = default!;

        [JsonProperty(PropertyName = "qr_data")]
        public string QrData {get; set;} = default!;

        public static implicit operator MercadoPagoQrCodeDto(PedidoResponse pedidoResponse){
            return new MercadoPagoQrCodeDto{
                DadoDoCodigo = pedidoResponse.QrData
            };
        }
    }
}