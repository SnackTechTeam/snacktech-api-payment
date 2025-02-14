using common.Api;
using common.DataSource;

namespace core.interfaces
{
    public interface IPagamentoController
    {
        Task<ResultadoOperacao<PagamentoDto>> CriarPagamento(PedidoDto pedido);
        Task<ResultadoOperacao<PagamentoDto>> CriarPagamentoMock(PedidoDto pedido);
        Task<ResultadoOperacao> ProcessarPagamento(PagamentoProcessadoDto pagamento);
        Task<ResultadoOperacao> ProcessarPagamentoMock(Guid identificacaoPedido);
    }
}