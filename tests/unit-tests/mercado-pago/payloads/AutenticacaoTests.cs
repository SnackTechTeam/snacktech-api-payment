using driver.mercado_pago.payloads;

namespace unit_tests.mercadopago.payloads
{
    public class AutenticacaoTests
    {

        [Fact]
        public void NaoCriarObjetoAutenticacaoComoEsperadoQuandoDadosNulos(){
            
            Assert.Throws<ArgumentNullException>(() => Autenticacao.CriarPayload("",null));
            Assert.Throws<ArgumentNullException>(() => Autenticacao.CriarPayload(null,""));
        }   

        [Theory]
        [InlineData("","")]
        [InlineData("id","secret")]
        public void CriarObjetoAutenticacaoComoEsperado(string clientId, string clientSecret){
            var autenticacaoObj = Autenticacao.CriarPayload(clientId,clientSecret);

            Assert.NotNull(autenticacaoObj);
            Assert.Equal(clientId,autenticacaoObj.ClientId);
            Assert.Equal(clientSecret,autenticacaoObj.ClientSecret);
            Assert.Equal("client_credentials",autenticacaoObj.GrantType);
            Assert.Equal("false",autenticacaoObj.TestToken);
        }
    }
}