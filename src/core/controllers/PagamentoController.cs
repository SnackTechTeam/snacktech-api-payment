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
    //TODO: Injetar DataSource quando colocar o MongoDB
    public class PagamentoController(IMercadoPagoIntegration mercadoPagoIntegration, 
                                        IOptions<MercadoPagoOptions> mercadoPagoOptions) : IPagamentoController
    {
        public async Task<ResultadoOperacao> ProcessarPagamento(PagamentoProcessadoDto pagamento){
            var mercadoPagoGateway = new MercadoPagoGateway(mercadoPagoIntegration,mercadoPagoOptions.Value);

            var resultado = await PagamentosUseCase.ProcessarPagamentoRealizado(mercadoPagoGateway,pagamento);

            return resultado;
        }

        public async Task<ResultadoOperacao> ProcessarPagamentoMock(Guid identificacaoPedido){
            var resultado = await PagamentosUseCase.ProcessarPagamentoViaMock(identificacaoPedido);
            return resultado;
        }

        public async Task<ResultadoOperacao<PagamentoDto>> CriarPagamento(PedidoDto pedido){
            var mercadoPagoGateway = new MercadoPagoGateway(mercadoPagoIntegration,mercadoPagoOptions.Value);
            var resultado = await PagamentosUseCase.GerarPagamentoAtravesDePedido(pedido,mercadoPagoGateway);
            return resultado;
        }
    }
}