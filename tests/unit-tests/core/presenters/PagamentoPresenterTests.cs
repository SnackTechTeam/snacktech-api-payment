using common.ApiSource.MercadoPago;
using core.presenters;
using unit_tests.helpers;

namespace unit_tests.core.presenters
{
    public class PagamentoPresenterTests
    {
        [Fact]
        public void ApresentarResultadoPagamento_WithValidInputs_ReturnsExpectedResult()
        {
            // Arrange
            var pedido = ObjectsBuilder.BuildPedidoDto();
            var dadoPagamento = new MercadoPagoQrCodeDto
            {
                DadoDoCodigo = "QRCODE",
                ValorPagamento = 50.20m
            };

            // Act
            var resultado = PagamentoPresenter.ApresentarResultadoPagamento(pedido, dadoPagamento);

            // Assert
            Assert.NotNull(resultado);
            Assert.False(resultado.TeveExcecao());
            Assert.True(resultado.TeveSucesso());
            Assert.NotNull(resultado.Dados);
            Assert.Equal(pedido.PedidoId, resultado.Dados.Id);
            Assert.Equal(dadoPagamento.DadoDoCodigo, resultado.Dados.QrCode);
            Assert.Equal(dadoPagamento.ValorPagamento, resultado.Dados.ValorTotal);
        }
    }
}