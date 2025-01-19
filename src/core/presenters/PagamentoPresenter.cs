using common.Api;
using common.ApiSource.MercadoPago;
using common.DataSource;

namespace core.presenters
{
    internal class PagamentoPresenter
    {
        internal static ResultadoOperacao<PagamentoDto> ApresentarResultadoPagamento(PedidoDto pedido,MercadoPagoQrCodeDto dadoPagamento){
            var pagamentoDto = new PagamentoDto{
                Id = pedido.Id,
                QrCode = dadoPagamento.DadoDoCodigo,
                ValorTotal = dadoPagamento.ValorPagamento
            };

            return new ResultadoOperacao<PagamentoDto>(pagamentoDto);
        }
    }
}