using common.Enums;
using common.ExternalSource.MongoDb;
using common.Interfaces;
using driver.database.mongo.Entities;
using MongoDB.Driver;

namespace driver.database.mongo.Repositories
{
    public class PagamentoRepository : IMongoDbIntegration
    {
        private readonly IMongoCollection<Pagamento> collection;

        public PagamentoRepository(IMongoDatabase database){
            collection = database.GetCollection<Pagamento>("Pagamentos");
            ConfigurarIndices();
        }

        private void ConfigurarIndices(){
            var builder = Builders<Pagamento>.IndexKeys;

            var indexModelId = new CreateIndexModel<Pagamento>(builder.Ascending(p => p.Id));
            var indexModelPedidoId = new CreateIndexModel<Pagamento>(builder.Ascending(p => p.PedidoId));
            var indexModelClienteId = new CreateIndexModel<Pagamento>(builder.Ascending(p => p.Cliente.ClienteId));

            var indexModelStatusData = new CreateIndexModel<Pagamento>(builder.Combine(
                builder.Ascending(p => p.Status),
                builder.Descending(p => p.DataCriacao)
            ));

            collection.Indexes.CreateMany(new[] {indexModelId,indexModelPedidoId,indexModelClienteId,indexModelStatusData});
        }

        public async Task<BuscaPagamentoDto?> BuscarPagamentoPorPedidoId(string pedidoId){
            var pagamentos = await collection.FindAsync(c => c.PedidoId == pedidoId);
            var pagamento = await pagamentos.FirstOrDefaultAsync();
            return pagamento is null?default:pagamento;
        }

        public async Task CriarPagamento(PagamentoEntityDto pagamentoDto,StatusPagamento statusPagamento){
            Pagamento pagamento = Pagamento.ConverterParaPagamento(pagamentoDto,statusPagamento);
            await collection.InsertOneAsync(pagamento);
        }

        public async Task<bool> AtualizarStatusPagamentoPorPedidoId(string pedidoId, StatusPagamento novoStatus, DateTime dataAtualizacao){

            var filtro = Builders<Pagamento>.Filter.Eq(p => p.PedidoId, pedidoId);
            
            var atualizacao = Builders<Pagamento>.Update
                                .Set(p => p.Status, novoStatus.ToString())
                                .Set(p => p.DataUltimaAtualizacao, dataAtualizacao);


            var resultado = await collection.UpdateOneAsync(filtro,atualizacao);

            return resultado.ModifiedCount > 0;
        }


    }
}