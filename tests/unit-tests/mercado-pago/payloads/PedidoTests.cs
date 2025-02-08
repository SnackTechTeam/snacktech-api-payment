using common.DataSource;
using driver.mercado_pago.payloads;
using unit_tests.helpers;

namespace unit_tests.mercadopago.payloads
{
    public class PedidoTests
    {
        [Fact]
        public void CriarPedidoItemCorretamente()
        {
            var pedidoItensDto = new List<PedidoItemDto>(){
                ObjectsBuilder.BuildPedidoItemDto(10.0M),
                ObjectsBuilder.BuildPedidoItemDto(30.0M)
            };
            var valorEsperadoProduto1 = 10;
            var valorEsperadoProduto2 = 30;
            var valorTotalEsperado = valorEsperadoProduto1 + valorEsperadoProduto2;
            var pedidoItem = new PedidoItem(pedidoItensDto);

            Assert.True(pedidoItem.UnitPrice == valorTotalEsperado);
            Assert.True(pedidoItem.TotalAmount == valorTotalEsperado);
            Assert.True(pedidoItem.SkuNumber == "produto-001");
            Assert.True(pedidoItem.Category == "Combo");
            Assert.True(pedidoItem.Title == "Combo SnackTech");
            Assert.True(pedidoItem.Description == "Conjunto de produtos da lanchonete SnackTeck");
            Assert.True(pedidoItem.Quantity == 1);
            Assert.True(pedidoItem.UnitMeasure == "unit");
        }

        [Fact]
        public void CriarPedidoCorretamente(){
            var pedidoItensDto = new List<PedidoItemDto>(){
                 ObjectsBuilder.BuildPedidoItemDto(10.0M),
                ObjectsBuilder.BuildPedidoItemDto(30.0M)
            };
            var pedidoDto = new PedidoDto{
                PedidoId = Guid.NewGuid(),
                Cliente = ObjectsBuilder.BuildClienteDto(),
                Itens = pedidoItensDto
            };

            var valorTotalEsperado = pedidoItensDto.Sum(p => p.Valor);
            var pedido = new Pedido(pedidoDto);

            Assert.Contains("Pedido",pedido.Title);
            Assert.Contains("SnackTech-Pedido-",pedido.Description);
            Assert.True(pedido.ExternalReference == pedidoDto.PedidoId.ToString());
            Assert.Equal(pedido.TotalAmount, valorTotalEsperado);
        }
    }
}