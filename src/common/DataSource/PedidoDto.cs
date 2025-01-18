namespace common.DataSource
{
    public class PedidoDto
    {
        public Guid Id {get; set;}
        public DateTime DataCriacao {get; set;}
        public int Status {get; set;}
        public required ClienteDto Cliente {get; set;}
        public required IEnumerable<PedidoItemDto> Itens {get; set;}
    }
}