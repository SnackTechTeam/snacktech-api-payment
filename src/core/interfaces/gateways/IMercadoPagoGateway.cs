using common.ApiSource.MercadoPago;
using common.DataSource;

namespace core.interfaces.gateways
{
    public interface IMercadoPagoGateway
    {
        Task<MercadoPagoQrCodeDto> IntegrarPedido(PedidoDto pedidoDto);
        Task<Guid> BuscarPedidoViaOrder(string orderId);
    }
}