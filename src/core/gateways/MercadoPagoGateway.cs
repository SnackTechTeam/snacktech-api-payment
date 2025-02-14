using System.Diagnostics.CodeAnalysis;
using common.ApiSource.MercadoPago;
using common.DataSource;
using common.Interfaces;
using common.Options;
using core.interfaces.gateways;

namespace core.gateways
{
    internal class MercadoPagoGateway(IMercadoPagoIntegration apiMercadoPago, MercadoPagoOptions mercadoPagoOptions) : IMercadoPagoGateway
    {
        internal async Task<MercadoPagoQrCodeDto> IntegrarPedido(PedidoDto pedidoDto){
            var autenticacao = await apiMercadoPago.Autenticar(mercadoPagoOptions);
            var resposta = await apiMercadoPago.GerarQrCode(autenticacao.TokenDeAcesso, mercadoPagoOptions, pedidoDto);

            return resposta;
        }

        internal async Task<Guid> BuscarPedidoViaOrder(string orderId){
            var autenticacao = await apiMercadoPago.Autenticar(mercadoPagoOptions);
            var resposta = await apiMercadoPago.BuscarOrdemPagamento(autenticacao.TokenDeAcesso,mercadoPagoOptions,orderId);

            return resposta;
        }

        [ExcludeFromCodeCoverage]
        Task<MercadoPagoQrCodeDto> IMercadoPagoGateway.IntegrarPedido(PedidoDto pedidoDto)
        {
            return IntegrarPedido(pedidoDto);
        }

        [ExcludeFromCodeCoverage]
        Task<Guid> IMercadoPagoGateway.BuscarPedidoViaOrder(string orderId)
        {
            return BuscarPedidoViaOrder(orderId);
        }
    }
}