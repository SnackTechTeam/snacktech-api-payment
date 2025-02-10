using core.presenters;

namespace unit_tests.core.presenters
{
    public class GeralPresenterTests
    {
        [Fact]
        public void ApresentarResultadoErroLogicoTipoGenerico()
        {
            string mensagem = "Erro lógico";
            var resultado = GeralPresenter.ApresentarResultadoErroLogico<string>(mensagem);

            Assert.NotNull(resultado);
            Assert.False(resultado.TeveExcecao());
            Assert.False(resultado.TeveSucesso());
            Assert.Equal(mensagem, resultado.Mensagem);
        }

        [Fact]
        public void ApresentarResultadoErroLogico()
        {
            string mensagem = "Erro lógico";
            var resultado = GeralPresenter.ApresentarResultadoErroLogico(mensagem);

            Assert.NotNull(resultado);
            Assert.Equal(mensagem, resultado.Mensagem);
        }

        [Fact]
        public void ApresentarResultadoErroInternoTipoGenerico()
        {
            var excecao = new Exception("Erro interno");
            var resultado = GeralPresenter.ApresentarResultadoErroInterno<string>(excecao);

            Assert.NotNull(resultado);
            Assert.True(resultado.TeveExcecao());
            Assert.Equal(excecao.Message, resultado.Mensagem);
        }

        [Fact]
        public void ApresentarResultadoErroInterno()
        {
            var excecao = new Exception("Erro interno");
            var resultado = GeralPresenter.ApresentarResultadoErroInterno(excecao);

            Assert.NotNull(resultado);
            Assert.True(resultado.TeveExcecao());
            Assert.Equal(excecao.Message, resultado.Mensagem);
        }

        [Fact]
        public void ApresentarResultadoPadraoSucesso()
        {
            var resultado = GeralPresenter.ApresentarResultadoPadraoSucesso();

            Assert.NotNull(resultado);
            Assert.False(resultado.TeveExcecao());
            Assert.True(resultado.TeveSucesso());
            Assert.Equal(string.Empty,resultado.Mensagem);
        }
    }
}