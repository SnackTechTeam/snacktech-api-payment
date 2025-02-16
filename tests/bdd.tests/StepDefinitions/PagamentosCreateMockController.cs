using api.Controllers;
using common.Api;
using common.DataSource;
using core.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace bdd.tests.StepDefinitions
{
    [Binding]
    public class PagamentosCreateMockController
    {
        private readonly PagamentosController controller;
        private readonly Mock<ILogger<PagamentosController>> loggerMock;
        private readonly Mock<IPagamentoController> pagamentoControllerMock;
        private PedidoDto pedidoDto;
        private IActionResult resultado;
        private PagamentoDto pagamentoRetorno;

        public PagamentosCreateMockController()
        {
            loggerMock = new Mock<ILogger<PagamentosController>>();
            pagamentoControllerMock = new Mock<IPagamentoController>();
            controller = new PagamentosController(loggerMock.Object, pagamentoControllerMock.Object);
        }

        [Given(@"eu tenho um Pedido preenchido corretamente")]
        public void DadoEuTenhoUmPedidoPreenchidoCorretamente()
        {
            pedidoDto = new PedidoDto
            {
                PedidoId = Guid.NewGuid(),
                Cliente = new ClienteDto
                {
                    Id = Guid.NewGuid(),
                    Nome = "Cliente Test",
                    Email = "email@gmail.com"
                },
                Itens = new List<PedidoItemDto>
                {
                    new PedidoItemDto
                    {
                        PedidoItemId = Guid.NewGuid(),
                        Valor = 10
                    }
                }
            };

            var pagamentoRetorno = new PagamentoDto{
                Id = Guid.NewGuid(),
                QrCode = $"0020101021243650016COM.MERCADOLIBRE020130636{pedidoDto.PedidoId}5204000053039865802BR5909Test Test6009SAO PAULO62970503***63449B90",
                ValorTotal = pedidoDto.Itens.Sum(i => i.Valor)
            };
            var retorno = new ResultadoOperacao<PagamentoDto>(pagamentoRetorno);
            pagamentoControllerMock.Setup(p => p.CriarPagamentoMock(pedidoDto))
                                .ReturnsAsync(retorno);
        }

        [When(@"eu chamar o método CriarPagamentoMock")]
        public async Task QuandoEuChamarOMetodoCriarPagamentoMock()
        {
            resultado = await controller.CriarPagamentoMock(pedidoDto);
        }

        [Then(@"eu devo receber um retorno IActionResult do tipo OkObjectResult")]
        public void EntaoEuDevoReceberUmRetornoIActionOkObjectResult()
        {
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            pagamentoRetorno = Assert.IsType<PagamentoDto>(okResult.Value);
            
        }
        
        [Then(@"a resposta deve ser do tipo PagamentoDto com os campos preenchidos")]
        public void ThenARespostaDeveSerDoTipoPagamentoDtoComCamposPreenchidos()
        {
            Assert.NotNull(pagamentoRetorno);
            Assert.NotEqual(Guid.Empty, pagamentoRetorno.Id);
            Assert.NotEmpty(pagamentoRetorno.QrCode);
            Assert.Equal(pedidoDto.Itens.Sum(i => i.Valor), pagamentoRetorno.ValorTotal);
        }
    }
}