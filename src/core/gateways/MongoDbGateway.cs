using common.Enums;
using common.ExternalSource.MongoDb;
using common.Interfaces;

namespace core.gateways
{
    internal class MongoDbGateway(IMongoDbIntegration mongoDbIntegration)
    {
        internal async Task GravarPagamento(PagamentoEntityDto pagamentoEntityDto){
            await mongoDbIntegration.CriarPagamento(pagamentoEntityDto);
        }    

        internal async Task<bool> AtualizarPagamentoPorPedidoId(Guid pedidoId, StatusPagamento statusPagamento, DateTime dataAtualizacao){
            var resultado = await mongoDbIntegration.AtualizarStatusPagamentoPorPedidoId(pedidoId.ToString(),statusPagamento,dataAtualizacao);
            return resultado;
        }

        internal async Task<BuscaPagamentoDto?> BuscarPagamentoPorPedidoId(Guid pedidoId){
            var resultado = await mongoDbIntegration.BuscarPagamentoPorPedidoId(pedidoId.ToString());
            return resultado;
        }
    }
}