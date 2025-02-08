
using common.Enums;
using common.ExternalSource.MongoDb;

namespace core.interfaces.gateways
{
    public interface IMongoDbGateway
    {
        Task GravarPagamento(PagamentoEntityDto pagamentoEntityDto);
        Task<bool> AtualizarPagamentoPorPedidoId(Guid pedidoId, StatusPagamento statusPagamento, DateTime dataAtualizacao);
        Task<BuscaPagamentoDto?> BuscarPagamentoPorPedidoId(Guid pedidoId);
    }
}