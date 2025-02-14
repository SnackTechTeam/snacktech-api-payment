using common.ApiSource.MercadoPago;
using driver.mercado_pago.payloads;

namespace unit_tests.mercadopago.payloads
{
    public class PedidoResponseTests
    {
        [Fact]
        public void CriarMercadoPagoQrCodeDtoBaseadoNoPedidoResponseViaOperator()
        {
            var pedidoResponse = new PedidoResponse{
                InStoreOrderId = "c6498e37-3aa7-496d-a458-dee0256c7c75",
                QrData = "00020101021243650016COM.MERCADOLIBRE020130636c6498e37-3aa7-496d-a458-dee0256c7c755204000053039865802BR5909Test Test6009SAO PAULO62070503***63042E33"
            };

            MercadoPagoQrCodeDto mercadoPagoQrCodeDto = pedidoResponse;

            Assert.Equal(mercadoPagoQrCodeDto.DadoDoCodigo,pedidoResponse.QrData);
        }
    }
}