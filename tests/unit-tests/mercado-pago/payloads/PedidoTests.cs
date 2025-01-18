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
            var produto1 = ObjectsBuilder.BuildProdutoDto(1,"produto","descricao",10.0M);
            var produto2 = ObjectsBuilder.BuildProdutoDto(3,"produto2","descricao",30.0M);
            var pedidoItensDto = new List<PedidoItemDto>(){
                ObjectsBuilder.BuildPedidoItemDto(produto1,2),
                ObjectsBuilder.BuildPedidoItemDto(produto2,1)
            };
            var valorEsperadoProduto1 = 2 * 10;
            var valorEsperadoProduto2 = 1 * 30;
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
            var produto1 = ObjectsBuilder.BuildProdutoDto(1,"produto","descricao",10.0M);
            var produto2 = ObjectsBuilder.BuildProdutoDto(3,"produto2","descricao",30.0M);
            var pedidoItensDto = new List<PedidoItemDto>(){
                ObjectsBuilder.BuildPedidoItemDto(produto1,2),
                ObjectsBuilder.BuildPedidoItemDto(produto2,1)
            };
            var pedidoDto = new PedidoDto{
                Id = Guid.NewGuid(),
                DataCriacao = DateTime.Now,
                Status = 1,
                Cliente = ObjectsBuilder.BuildClienteDto(),
                Itens = pedidoItensDto
            };

            var valorTotalEsperado = pedidoItensDto.Sum(p => p.Produto.Valor * p.Quantidade);
            var pedido = new Pedido(pedidoDto);

            Assert.Contains("Pedido",pedido.Title);
            Assert.Contains("SnackTech-Pedido-",pedido.Description);
            Assert.True(pedido.ExternalReference == pedidoDto.Id.ToString());
            Assert.Equal(pedido.TotalAmount, valorTotalEsperado);
        }
    }
}