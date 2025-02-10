using common.ExternalSource.Sqs;
using core.domain;
using core.domain.types;

namespace unit_tests.core.domain
{
    public class PagamentoPedidoTests
    {
        [Fact]
        public void ConstrutorInicializandoPropriedadesComoEsperado()
        {
            // Arrange
            var pedidoId = new GuidValido(Guid.NewGuid());
            var pagamentoId = new StringNaoVaziaOuComEspacos("12345");
            var dataPagamento = new DataValida(DateTime.Now);
            var nomePlataforma = new StringNaoVaziaOuComEspacos("MercadoPago");

            // Act
            var pagamentoPedido = new PagamentoPedido(pedidoId, pagamentoId, dataPagamento, nomePlataforma);

            // Assert
            Assert.Equal(pedidoId, pagamentoPedido.PedidoId);
            Assert.Equal(pagamentoId, pagamentoPedido.PagamentoId);
            Assert.Equal(dataPagamento, pagamentoPedido.DataRecebimentoPagamento);
            Assert.Equal(nomePlataforma, pagamentoPedido.NomePlataformaPagamento);
        }

        [Fact]
        public void ConverterParaPagamentoMessageDto()
        {
            // Arrange
            var pedidoId = new GuidValido(Guid.NewGuid());
            var pagamentoId = new StringNaoVaziaOuComEspacos("12345");
            var dataPagamento = new DataValida(DateTime.Now);
            var nomePlataforma = new StringNaoVaziaOuComEspacos("Mercado");
            var pagamentoPedido = new PagamentoPedido(pedidoId, pagamentoId, dataPagamento, nomePlataforma);

            // Act
            PagamentoMessageDto dto = pagamentoPedido;

            // Assert
            Assert.Equal(pedidoId.Valor.ToString(), dto.PedidoId);
            Assert.Equal(pagamentoId, dto.PagamentoId);
            Assert.Equal(dataPagamento.Valor, dto.DataRecebimento);
            Assert.Equal(nomePlataforma, dto.NomePlataforma);
        }
    }
}