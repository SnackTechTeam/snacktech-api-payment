using common.ExternalSource.Sqs;
using core.domain.types;

namespace core.domain
{
    internal class PagamentoPedido
    {
        internal GuidValido PedidoId {get; set;}        
        internal StringNaoVaziaOuComEspacos PagamentoId {get; set;}
        internal DataValida DataRecebimentoPagamento {get; set;}
        internal StringNaoVaziaOuComEspacos NomePlataformaPagamento {get; set;}

        public PagamentoPedido(Guid pedidoId, string pagamentoId, DateTime dataPagamento, string nomePlataforma){
            PedidoId = pedidoId;
            PagamentoId = pagamentoId;
            DataRecebimentoPagamento = dataPagamento;
            NomePlataformaPagamento = nomePlataforma;
        }

        public static implicit operator PagamentoMessageDto(PagamentoPedido pagamentoPedido)
            => new PagamentoMessageDto{
                PedidoId = pagamentoPedido.PedidoId.Valor.ToString(),
                PagamentoId = pagamentoPedido.PagamentoId,
                DataRecebimento = pagamentoPedido.DataRecebimentoPagamento.Valor,
                NomePlataforma = pagamentoPedido.NomePlataformaPagamento
            };
    }
}