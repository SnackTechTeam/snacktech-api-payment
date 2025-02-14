using common.Api;
using common.ApiSource.MercadoPago;
using common.DataSource;
using common.Enums;
using common.ExternalSource.MongoDb;
using core.domain;
using core.domain.types;
using core.interfaces.gateways;
using core.presenters;

namespace core.usecases
{
    internal static class PagamentosUseCase
    {
        internal static async Task<ResultadoOperacao<PagamentoDto>> GerarPagamentoAtravesDePedido(IMercadoPagoGateway mercadoPagoGateway, IMongoDbGateway mongoDbGateway,PedidoDto pedido){
            try{
                var dadoPagamento = await mercadoPagoGateway.IntegrarPedido(pedido);
                var entidadePagamento = new PagamentoEntityDto{
                    pedidoDto = pedido,
                    pagamentoDto = dadoPagamento
                };
                await mongoDbGateway.GravarPagamento(entidadePagamento,StatusPagamento.Pendente);
                return PagamentoPresenter.ApresentarResultadoPagamento(pedido,dadoPagamento);
            }
            catch(Exception ex){
                return GeralPresenter.ApresentarResultadoErroInterno<PagamentoDto>(ex);
            }
        }

        internal static async Task<ResultadoOperacao<PagamentoDto>> GerarPagamentoViaMock(IMongoDbGateway mongoDbGateway, ISqsGateway sqsGateway, PedidoDto pedido){
            try{
                var dadoPagamento = new MercadoPagoQrCodeDto{
                        DadoDoCodigo = "00020101021243650016COM.FAKEDATA020130636974f89bd-380b-4d40-a7d3-320d852e82145204000053039865802BR5909Teste Tes6009SAO PAULO62070503***63043449",
                        LojaPedidoId = "loja-pedido-id",
                        ValorPagamento = pedido.Itens.Sum(i => i.Valor)
                    };
                var entidadePagamento = new PagamentoEntityDto{
                    pedidoDto = pedido,
                    pagamentoDto = dadoPagamento
                };
                await mongoDbGateway.GravarPagamento(entidadePagamento,StatusPagamento.Concluido);
                BuscaPagamentoDto pagamentoGravado = await mongoDbGateway.BuscarPagamentoPorPedidoId(pedido.PedidoId) ?? throw new ArgumentException("Pagamento não encontrado na base de dados.");
                var dataDeAtualizacao = DateTime.Now;
                var pagamentoPedido = new PagamentoPedido(pedido.PedidoId,pagamentoGravado.PagamentoId,dataDeAtualizacao,"MercadoPago");

                await sqsGateway.PublicarMensagemPagamentoNoSqs(pagamentoPedido);

                return PagamentoPresenter.ApresentarResultadoPagamento(pedido,dadoPagamento);
            }
            catch(Exception ex){
                return GeralPresenter.ApresentarResultadoErroInterno<PagamentoDto>(ex);
            }
        }

        internal static async Task<ResultadoOperacao> ProcessarPagamentoRealizado(IMercadoPagoGateway mercadoPagoGateway, IMongoDbGateway mongoDbGateway, ISqsGateway sqsGateway, PagamentoProcessadoDto pagamento){
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

                var pagamentoPedido = new PagamentoPedido(referencia,pagamentoGravado.PagamentoId,dataDeAtualizacao,"MercadoPago");

                await sqsGateway.PublicarMensagemPagamentoNoSqs(pagamentoPedido);

                return GeralPresenter.ApresentarResultadoPadraoSucesso();
            }
            catch(ArgumentException ex){
                return GeralPresenter.ApresentarResultadoErroLogico(ex.Message);
            }
            catch(Exception ex){
                return GeralPresenter.ApresentarResultadoErroInterno(ex);
            }
        }

        internal static async Task<ResultadoOperacao> ProcessarPagamentoViaMock(IMongoDbGateway mongoDbGateway, ISqsGateway sqsGateway,Guid identificacaoPedido){
            try{
               var pagamentoGravado = await mongoDbGateway.BuscarPagamentoPorPedidoId(identificacaoPedido);

                if(pagamentoGravado is null)
                    return GeralPresenter.ApresentarResultadoErroLogico($"Não foi encontrado pagamento gravado para pedido {identificacaoPedido}");

                var dataDeAtualizacao = DateTime.Now;
                
                var atualizacaoBase = await mongoDbGateway.AtualizarPagamentoPorPedidoId(identificacaoPedido,StatusPagamento.Concluido,dataDeAtualizacao);

                if(!atualizacaoBase)
                    return GeralPresenter.ApresentarResultadoErroLogico($"Não foi possível atualizar pagamento do pedido {identificacaoPedido} na base.");

                var pagamentoPedido = new PagamentoPedido(identificacaoPedido,pagamentoGravado.PagamentoId,dataDeAtualizacao,"MercadoPago");

                await sqsGateway.PublicarMensagemPagamentoNoSqs(pagamentoPedido);

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