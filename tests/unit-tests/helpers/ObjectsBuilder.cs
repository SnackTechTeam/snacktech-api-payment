using common.DataSource;
using common.Options;

namespace unit_tests.helpers
{
    public static class ObjectsBuilder
    {
        public static PedidoItemDto BuildPedidoItemDto(decimal valor, int quantidade = 1)
        => new PedidoItemDto{
            PedidoItemId = Guid.NewGuid(),
            Valor = valor
        };

        public static ClienteDto BuildClienteDto()
        => new ClienteDto{
            Email = "email@google.com",
            Id = Guid.NewGuid(),
            Nome = "Nome Cliente"
        };

        public static PedidoDto BuildPedidoDto()
        {
            var pedidoItensDto = new List<PedidoItemDto>(){
                ObjectsBuilder.BuildPedidoItemDto(10.0M,2),
                ObjectsBuilder.BuildPedidoItemDto(30.0M,1)
            };
            return new PedidoDto{
                PedidoId = Guid.NewGuid(),
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