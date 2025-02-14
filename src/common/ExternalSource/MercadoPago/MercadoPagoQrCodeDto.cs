namespace common.ApiSource.MercadoPago
{
    public class MercadoPagoQrCodeDto
    {
        public string LojaPedidoId {get; set;} = default!;
        public string DadoDoCodigo {get; set;} = default!;
        public decimal ValorPagamento {get; set;}
    }
}