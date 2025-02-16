using api.Controllers;
using common.Api;
using core.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace bdd.tests.StepDefinitions
{
    [Binding]
    public class PagamentosProcessarPagamentoStepDefinitions
    {
        private readonly PagamentosController controller;
        private readonly Mock<ILogger<PagamentosController>> loggerMock;
        private readonly Mock<IPagamentoController> pagamentoControllerMock;
        private IActionResult resultado;
        private PagamentoProcessadoDto pagamentoProcessadoDto;
        private Guid pedidoId;

        public PagamentosProcessarPagamentoStepDefinitions()
        {
            loggerMock = new Mock<ILogger<PagamentosController>>();
            pagamentoControllerMock = new Mock<IPagamentoController>();
            controller = new PagamentosController(loggerMock.Object, pagamentoControllerMock.Object);
        }

        [Given(@"um pagamento retornado pela plataforma")]
        public void DadoUmPagamentoRetornadoPelaPlataforma(){
            pagamentoProcessadoDto = new PagamentoProcessadoDto
            {
                Action = "update",
                ApplicationId = "7910800073137785",
                Data = new PagamentoProcessadoDataDto
                {
                    CurrencyId = "",
                    Marketplace = "NONE",
                    Status = "closed"
                },
                DateCreated = "2024-10-06T17:42:13.510-04:00",
                Id = "23603350837",
                LiveMode = false,
                Status = "closed",
                Type = "topic_merchant_order_wh",
                UserId = 2012037660,
                Version = 1
            };

            var retorno = new ResultadoOperacao();
            pagamentoControllerMock.Setup(p => p.ProcessarPagamento(pagamentoProcessadoDto))
                                .ReturnsAsync(retorno);
        }

        [Given(@"um Id de pedido válido")]
        public void DadoUmIdDePedidoValido(){
            pedidoId = Guid.NewGuid();
            var retorno = new ResultadoOperacao();
            pagamentoControllerMock.Setup(p => p.ProcessarPagamentoMock(pedidoId))
                                .ReturnsAsync(retorno);
        }

        
        [When(@"for chamado o método PagamentoHook")]
        public async Task QuandoForChamadoOMetodoPagamentoHook(){
            resultado = await controller.PagamentoHook(pagamentoProcessadoDto);
        }

        [When(@"for chamado o método PagamentoMock")]
        public async Task QuandoForChamadoOMetodoPagamentoMock(){
            resultado = await controller.PagamentoMock(pedidoId);
        }

        [Then(@"deve retornar um objeto OkResult")]
        public void DeveRetornarUmObjetoOkResult(){
            Assert.IsType<OkResult>(resultado);
        }
    }
}