using core.domain.types;

namespace core.domain
{
    internal class PagamentoPedido
    {
        internal GuidValido PedidoId {get; set;}        
        internal GuidValido PagamentoId {get; set;}
        internal DataValida DataRecebimentoPagamento {get; set;}
        internal StringNaoVaziaOuComEspacos NomePlataformaPagamento {get; set;}

        public PagamentoPedido(Guid pedidoId, Guid pagamentoId, DateTime dataPagamento, string nomePlataforma){
            PedidoId = pedidoId;
            PagamentoId = pagamentoId;
            DataRecebimentoPagamento = dataPagamento;
            NomePlataformaPagamento = nomePlataforma;
        }
    }
}