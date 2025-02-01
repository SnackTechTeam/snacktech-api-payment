using common.Api;
using common.DataSource;
using common.Interfaces;
using common.Options;
using core.gateways;
using core.interfaces;
using core.usecases;
using Microsoft.Extensions.Options;

namespace core.controllers
{
    public class PagamentoController(IMercadoPagoIntegration mercadoPagoIntegration, 
                                        IMongoDbIntegration mongoDbIntegration, 
                                        ISqsIntegration sqsIntegration,
                                        IOptions<MercadoPagoOptions> mercadoPagoOptions) : IPagamentoController
    {
        public async Task<ResultadoOperacao> ProcessarPagamento(PagamentoProcessadoDto pagamento){
            var mercadoPagoGateway = new MercadoPagoGateway(mercadoPagoIntegration,mercadoPagoOptions.Value);
            var mongoDbGateway = new MongoDbGateway(mongoDbIntegration);
            var sqsGateway = new SqsGateway(sqsIntegration);
            var resultado = await PagamentosUseCase.ProcessarPagamentoRealizado(mercadoPagoGateway,mongoDbGateway,sqsGateway,pagamento);

            return resultado;
        }

        public async Task<ResultadoOperacao> ProcessarPagamentoMock(Guid identificacaoPedido){
            var mongoDbGateway = new MongoDbGateway(mongoDbIntegration);
            var sqsGateway = new SqsGateway(sqsIntegration);
            var resultado = await PagamentosUseCase.ProcessarPagamentoViaMock(mongoDbGateway,sqsGateway,identificacaoPedido);
            return resultado;
        }

        public async Task<ResultadoOperacao<PagamentoDto>> CriarPagamento(PedidoDto pedido){
            var mercadoPagoGateway = new MercadoPagoGateway(mercadoPagoIntegration,mercadoPagoOptions.Value);
            var mongoDbGateway = new MongoDbGateway(mongoDbIntegration);
            var resultado = await PagamentosUseCase.GerarPagamentoAtravesDePedido(mercadoPagoGateway,mongoDbGateway,pedido);
            return resultado;
        }
    }
}