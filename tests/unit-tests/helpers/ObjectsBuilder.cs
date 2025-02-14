using common.ApiSource.MercadoPago;
using common.DataSource;
using common.Options;
using driver.database.mongo.Entities;

namespace unit_tests.helpers
{
    public static class ObjectsBuilder
    {
        public static PedidoItemDto BuildPedidoItemDto(decimal valor)
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
                ObjectsBuilder.BuildPedidoItemDto(10.0M),
                ObjectsBuilder.BuildPedidoItemDto(30.0M)
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

        public static MercadoPagoQrCodeDto BuildMercadoPagoQrCodeDto()
        => new MercadoPagoQrCodeDto{
            DadoDoCodigo = "DadoDoCodigo",
            LojaPedidoId = "LojaPedidoId",
            ValorPagamento = 10.0M
        };

        public static Pagamento BuildPagamento()
        => new Pagamento{
            Cliente = ObjectsBuilder.BuildClienteDto(),
            DataCriacao = DateTime.Now,
            DataUltimaAtualizacao = null,
            Id = "123",
            LojaPedidoId = "LojaPedidoId",
            PedidoId = Guid.NewGuid().ToString(),
            QrCodePagamento = "QrCodePagamento",
            Status = "Status",
            Valor = 10.0M
        };
    }
}