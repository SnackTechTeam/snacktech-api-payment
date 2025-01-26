namespace common.DataSource
{
    public class PedidoDto
    {
        public Guid PedidoId {get; set;}
        public required ClienteDto Cliente {get; set;}
        public required IEnumerable<PedidoItemDto> Itens {get; set;}
    }
}