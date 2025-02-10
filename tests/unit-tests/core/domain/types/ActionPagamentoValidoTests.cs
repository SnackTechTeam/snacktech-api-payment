
using core.domain.types;

namespace unit_tests.core.domain.types
{
    public class ActionPagamentoValidoTests
    {
        [Theory]
        [InlineData("update")]
        [InlineData("create")]
        public void ValorCorretoNaoDeveLancarException(string validValue)
        {
            var exception = Record.Exception(() => new ActionPagamentoValido(validValue));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("delete")]
        [InlineData("invalid")]
        public void ValorIncorretoDeveLancarArgumentException(string invalidValue)
        {
            var actionPagamentoValido = new ActionPagamentoValido("create");

            var exception = Assert.Throws<ArgumentException>(() => actionPagamentoValido.Valor = invalidValue);
            Assert.Equal($"Valor {invalidValue} não é uma Categoria de Produto Válida", exception.Message);
        }

        [Theory]
        [InlineData("update")]
        [InlineData("create")]
        public void ConversaoCorretaDeStringParaActionPagamentoValido(string validValue)
        {
            ActionPagamentoValido actionPagamentoValido = validValue;

            Assert.Equal(validValue, actionPagamentoValido.Valor);
        }

        [Theory]
        [InlineData("update")]
        [InlineData("create")]
        public void ConversaoCorretaDeActionPagamentoValidoParaString(string validValue)
        {
            var actionPagamentoValido = new ActionPagamentoValido(validValue);

            string valor = actionPagamentoValido;

            Assert.Equal(validValue, valor);
        }

        [Theory]
        [InlineData("update")]
        [InlineData("create")]
        public void ToStringRetornandoValor(string validValue)
        {
            var actionPagamentoValido = new ActionPagamentoValido(validValue);

            string valor = actionPagamentoValido.ToString();

            Assert.Equal(validValue, valor);
        }
    }
}