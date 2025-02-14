using common.ApiSource.MercadoPago;
using driver.mercado_pago.payloads;

namespace unit_tests.mercadopago.payloads
{
    public class AutenticacaoResponseTests
    {
        [Fact]
        public void CriarAutenticacaoMercadoPagoDtoBaseadoEmAutenticacaoResponseVaiOperator()
        {
            var response = new AutenticacaoResponse{
                AccessToken = "APP_USR-",
                TokenType = "Bearer",
                ExpiresIn = 21600,
                Scope = "read write",
                UserId = 2000007700,
                LiveMode = true
            };

            AutenticacaoMercadoPagoDto dto = response;

            Assert.True(dto.IdUsuario == response.UserId.ToString());
            Assert.True(dto.TempoExpiracao == response.ExpiresIn);
            Assert.True(dto.TokenDeAcesso == response.AccessToken);
        }
    }
}