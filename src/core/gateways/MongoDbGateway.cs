using System.Diagnostics.CodeAnalysis;
using common.Enums;
using common.ExternalSource.MongoDb;
using common.Interfaces;
using core.interfaces.gateways;

namespace core.gateways
{
    internal class MongoDbGateway(IMongoDbIntegration mongoDbIntegration): IMongoDbGateway
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

        [ExcludeFromCodeCoverage]
        Task IMongoDbGateway.GravarPagamento(PagamentoEntityDto pagamentoEntityDto)
        {
            return GravarPagamento(pagamentoEntityDto);
        }

        [ExcludeFromCodeCoverage]
        Task<bool> IMongoDbGateway.AtualizarPagamentoPorPedidoId(Guid pedidoId, StatusPagamento statusPagamento, DateTime dataAtualizacao)
        {
            return AtualizarPagamentoPorPedidoId(pedidoId, statusPagamento, dataAtualizacao);
        }

        [ExcludeFromCodeCoverage]
        Task<BuscaPagamentoDto?> IMongoDbGateway.BuscarPagamentoPorPedidoId(Guid pedidoId)
        {
            return BuscarPagamentoPorPedidoId(pedidoId);
        }
    }
}