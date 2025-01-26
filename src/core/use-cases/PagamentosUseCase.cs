
using common.Api;
using common.DataSource;
using common.Enums;
using common.ExternalSource.MongoDb;
using core.domain;
using core.domain.types;
using core.gateways;
using core.presenters;

namespace core.usecases
{
    internal static class PagamentosUseCase
    {
        internal static async Task<ResultadoOperacao<PagamentoDto>> GerarPagamentoAtravesDePedido(MercadoPagoGateway mercadoPagoGateway, MongoDbGateway mongoDbGateway,PedidoDto pedido){
            try{
                var dadoPagamento = await mercadoPagoGateway.IntegrarPedido(pedido);
                var entidadePagamento = new PagamentoEntityDto{
                    pedidoDto = pedido,
                    pagamentoDto = dadoPagamento
                };
                await mongoDbGateway.GravarPagamento(entidadePagamento);
                return PagamentoPresenter.ApresentarResultadoPagamento(pedido,dadoPagamento);
            }
            catch(Exception ex){
                return GeralPresenter.ApresentarResultadoErroInterno<PagamentoDto>(ex);
            }
        }

        internal static async Task<ResultadoOperacao> ProcessarPagamentoRealizado(MercadoPagoGateway mercadoPagoGateway, MongoDbGateway mongoDbGateway, RabbitMqGateway rabbitMqGateway, PagamentoProcessadoDto pagamento){
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

                var pagamentoGravado = await mongoDbGateway.BuscarPagamentoPorPedidoId(referencia);

                if(pagamentoGravado is null)
                    return GeralPresenter.ApresentarResultadoErroLogico($"Não foi encontrado pagamento gravado para pedido {referencia}");

                var dataDeAtualizacao = DateTime.Now;
                
                var atualizacaoBase = await mongoDbGateway.AtualizarPagamentoPorPedidoId(referencia,StatusPagamento.Concluido,dataDeAtualizacao);

                if(!atualizacaoBase)
                    return GeralPresenter.ApresentarResultadoErroLogico($"Não foi possível atualizar pagamento do pedido {referencia} na base.");

                var pagamentoId = Guid.Parse(pagamentoGravado.PagamentoId);
                var pagamentoPedido = new PagamentoPedido(referencia,pagamentoId,dataDeAtualizacao,"MercadoPago");

                await rabbitMqGateway.PublicarMensagemPagamentoRealizado(pagamentoPedido);

                return GeralPresenter.ApresentarResultadoPadraoSucesso();
            }
            catch(ArgumentException ex){
                return GeralPresenter.ApresentarResultadoErroLogico(ex.Message);
            }
            catch(Exception ex){
                return GeralPresenter.ApresentarResultadoErroInterno(ex);
            }
        }

        internal static async Task<ResultadoOperacao> ProcessarPagamentoViaMock(MongoDbGateway mongoDbGateway, RabbitMqGateway rabbitMqGateway,Guid identificacaoPedido){
            try{
               var pagamentoGravado = await mongoDbGateway.BuscarPagamentoPorPedidoId(identificacaoPedido);

                if(pagamentoGravado is null)
                    return GeralPresenter.ApresentarResultadoErroLogico($"Não foi encontrado pagamento gravado para pedido {identificacaoPedido}");

                var dataDeAtualizacao = DateTime.Now;
                
                var atualizacaoBase = await mongoDbGateway.AtualizarPagamentoPorPedidoId(identificacaoPedido,StatusPagamento.Concluido,dataDeAtualizacao);

                if(!atualizacaoBase)
                    return GeralPresenter.ApresentarResultadoErroLogico($"Não foi possível atualizar pagamento do pedido {identificacaoPedido} na base.");

                var pagamentoId = Guid.Parse(pagamentoGravado.PagamentoId);
                var pagamentoPedido = new PagamentoPedido(identificacaoPedido,pagamentoId,dataDeAtualizacao,"MercadoPago");

                await rabbitMqGateway.PublicarMensagemPagamentoRealizado(pagamentoPedido);

                return GeralPresenter.ApresentarResultadoPadraoSucesso();
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