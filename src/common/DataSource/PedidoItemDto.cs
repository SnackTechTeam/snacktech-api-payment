namespace common.DataSource
{
    public class PedidoItemDto
    {
        public Guid Id {get; set;}
        public int Quantidade {get; set;}
        public decimal Valor {get; set;}
        public string Observacao {get; set;} = string.Empty;
        public required ProdutoDto Produto {get; set;}
    }
}