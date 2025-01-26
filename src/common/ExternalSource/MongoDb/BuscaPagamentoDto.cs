
namespace common.ExternalSource.MongoDb
{
    public class BuscaPagamentoDto
    {
        public string PagamentoId {get; set;} = default!;
        public string PedidoId {get; set;} = default!;
        public string ClienteId {get; set;} = default!;
        public DateTime DataCriacao {get; set;}
        public decimal Valor {get;set;}
        public string Status {get ;set;} = default!;
        public DateTime? DataUltimaAtualizacao {get; set;}
    }
}