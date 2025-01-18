using common.DataSource;
using common.Options;

namespace unit_tests.helpers
{
    public static class ObjectsBuilder
    {
        public static ProdutoDto BuildProdutoDto(int categoria, string nome, string descricao, decimal valor)
            => new ProdutoDto{
                Id = Guid.NewGuid(),
                Categoria = categoria,
                Nome = nome,
                Descricao = descricao,
                Valor = valor
            };

        public static PedidoItemDto BuildPedidoItemDto(ProdutoDto produtoDto, int quantidade = 1)
        => new PedidoItemDto{
            Id = Guid.NewGuid(),
            Quantidade = quantidade,
            Valor = quantidade * produtoDto.Valor,
            Observacao = "",
            Produto = produtoDto
        };

        public static ClienteDto BuildClienteDto()
        => new ClienteDto{
            Cpf = "74042277047",
            Email = "email@google.com",
            Id = Guid.NewGuid(),
            Nome = "Nome Cliente"
        };

        public static PedidoDto BuildPedidoDto()
        {
            var produto1 = ObjectsBuilder.BuildProdutoDto(1,"produto","descricao",10.0M);
            var produto2 = ObjectsBuilder.BuildProdutoDto(3,"produto2","descricao",30.0M);
            var pedidoItensDto = new List<PedidoItemDto>(){
                ObjectsBuilder.BuildPedidoItemDto(produto1,2),
                ObjectsBuilder.BuildPedidoItemDto(produto2,1)
            };
            return new PedidoDto{
                Id = Guid.NewGuid(),
                DataCriacao = DateTime.Now,
                Status = 1,
                Cliente = ObjectsBuilder.BuildClienteDto(),
                Itens = pedidoItensDto
            };
        }

        public static MercadoPagoOptions BuildMercadoPagoOptions()
        => new MercadoPagoOptions{
            ClientId = "clientId",
            ClientSecret = "clientSecret",
            PosId = "POS001",
            UrlBase = "https://localhost.com/",
            UserId = "2252139880"
        };
    }
}