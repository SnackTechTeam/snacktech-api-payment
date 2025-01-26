
using common.Api;
using common.DataSource;
using core.domain.types;
using core.gateways;
using core.presenters;

namespace core.usecases
{
    internal static class PagamentosUseCase
    {
        internal static async Task<ResultadoOperacao<PagamentoDto>> GerarPagamentoAtravesDePedido(PedidoDto pedido, MercadoPagoGateway mercadoPagoGateway){
            try{
                var dadoPagamento = await mercadoPagoGateway.IntegrarPedido(pedido);
                //TODO: Gravar pedido + dado pagamento no Mongo Db
                return PagamentoPresenter.ApresentarResultadoPagamento(pedido,dadoPagamento);
            }
            catch(Exception ex){
                return GeralPresenter.ApresentarResultadoErroInterno<PagamentoDto>(ex);
            }
        }

        internal static async Task<ResultadoOperacao> ProcessarPagamentoRealizado(MercadoPagoGateway mercadoPagoGateway, PagamentoProcessadoDto pagamento){
            try{
                ActionPagamentoValido acao = pagamento.Action;

                //validar status do pagamento
                //se for de abertura, só retorna sucesso por enquanto
                if(acao == "create")
                    return GeralPresenter.ApresentarResultadoPadraoSucesso();

                //validar que status está closed. Se estiver diferente não fazer nenhuma operação
                if(pagamento.Status != "closed")
                    return GeralPresenter.ApresentarResultadoPadraoSucesso();

                var referencia = await mercadoPagoGateway.BuscarPedidoViaOrder(pagamento.Id);

                //TODO: Processo que acionaria o microsserviço de pedido para atualizar status
                var statusProcessoAtualizacaoPedido = true;

                //TODO: Gravar que o pagamento foi recebido e que o pedido foi atualizado no MongoDB

                var retorno = statusProcessoAtualizacaoPedido ?
                                GeralPresenter.ApresentarResultadoPadraoSucesso():
                                GeralPresenter.ApresentarResultadoErroLogico($"Não foi possível atualizar pedido {referencia} após pagamento");

                return retorno;
            }
            catch(ArgumentException ex){
                return GeralPresenter.ApresentarResultadoErroLogico(ex.Message);
            }
            catch(Exception ex){
                return GeralPresenter.ApresentarResultadoErroInterno(ex);
            }
        }

        internal static async Task<ResultadoOperacao> ProcessarPagamentoViaMock(Guid identificacaoPedido){
            try{
                //TODO: Processo que acionaria o microsserviço de pedido para atualizar status
                await Task.FromResult(0);
                var statusProcessoAtualizacaoPedido = true;

                var retorno = statusProcessoAtualizacaoPedido?
                                GeralPresenter.ApresentarResultadoPadraoSucesso():
                                GeralPresenter.ApresentarResultadoErroLogico($"Não foi possível atualizar pedido {identificacaoPedido} após pagamento");

                return retorno;
            }
            catch(ArgumentException ex){
                return GeralPresenter.ApresentarResultadoErroLogico(ex.Message);
            }
            catch(Exception ex){
                return GeralPresenter.ApresentarResultadoErroInterno(ex);
            }
        }
    }
}