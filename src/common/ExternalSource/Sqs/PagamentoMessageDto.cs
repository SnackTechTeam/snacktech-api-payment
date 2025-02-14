namespace common.ExternalSource.Sqs
{
    public class PagamentoMessageDto
    {
        public string PedidoId {get; set;} = default!;
        public string PagamentoId {get; set;} = default!;
        public DateTime DataRecebimento {get; set;} = default!;
        public string NomePlataforma {get; set;} = default!;

    }
}