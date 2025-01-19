using common.ApiSource.MercadoPago;
using common.DataSource;
using common.Interfaces;
using common.Options;

namespace core.gateways
{
    public class MercadoPagoGateway(IMercadoPagoIntegration apiMercadoPago, MercadoPagoOptions mercadoPagoOptions)
    {
        internal async Task<MercadoPagoQrCodeDto> IntegrarPedido(PedidoDto pedidoDto){
            var autenticacao = await apiMercadoPago.Autenticar(mercadoPagoOptions);
            var resposta = await apiMercadoPago.GerarQrCode(autenticacao.TokenDeAcesso, mercadoPagoOptions,pedidoDto);

            return resposta;
        }

        internal async Task<Guid> BuscarPedidoViaOrder(string orderId){
            var autenticacao = await apiMercadoPago.Autenticar(mercadoPagoOptions);
            var resposta = await apiMercadoPago.BuscarOrdemPagamento(autenticacao.TokenDeAcesso,mercadoPagoOptions,orderId);

            return resposta;
        }
    }
}