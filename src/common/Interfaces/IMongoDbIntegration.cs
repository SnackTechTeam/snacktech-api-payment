
using common.Enums;
using common.ExternalSource.MongoDb;

namespace common.Interfaces
{
    public interface IMongoDbIntegration
    {
        Task CriarPagamento(PagamentoEntityDto pagamentoDto);
        Task<bool> AtualizarStatusPagamentoPorPedidoId(string pedidoId, StatusPagamento novoStatus, DateTime dataAtualizacao);
        Task<BuscaPagamentoDto?> BuscarPagamentoPorPedidoId(string pedidoId);
    }
}