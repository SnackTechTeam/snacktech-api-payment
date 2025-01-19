namespace common.ApiSource.MercadoPago
{
    public class MercadoPagoQrCodeDto
    {
        public string DadoDoCodigo {get; set;} = default!;
        public decimal ValorPagamento {get; set;}
    }
}