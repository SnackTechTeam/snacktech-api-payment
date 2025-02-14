using common.ApiSource.MercadoPago;
using common.DataSource;
using common.Options;

namespace common.Interfaces
{
    public interface IMercadoPagoIntegration
    {
        Task<AutenticacaoMercadoPagoDto> Autenticar(MercadoPagoOptions mercadoPagoOptions);
        Task<MercadoPagoQrCodeDto> GerarQrCode(string accessToken,MercadoPagoOptions mercadoPagoOptions, PedidoDto pedido);
        Task<Guid> BuscarOrdemPagamento(string accessToken,MercadoPagoOptions mercadoPagoOptions, string orderId); 
    }
}