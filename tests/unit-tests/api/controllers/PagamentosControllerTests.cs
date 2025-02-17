

using api.Controllers;
using api.CustomResponses;
using common.Api;
using common.DataSource;
using core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using unit_tests.helpers;

namespace unit_tests.api.controllers
{
    public class PagamentosControllerTests
    {
        private readonly Mock<ILogger<PagamentosController>> loggerMock;
        private readonly Mock<IPagamentoController> pagamentoDomainControllerMock;
        private readonly PagamentosController controller;

        public PagamentosControllerTests()
        {
            loggerMock = new Mock<ILogger<PagamentosController>>();
            pagamentoDomainControllerMock = new Mock<IPagamentoController>();
            controller = new PagamentosController(loggerMock.Object, pagamentoDomainControllerMock.Object);
        }

        [Fact]
        public async Task PagamentoMockOk()
        {
            var identificacao = Guid.NewGuid();
            pagamentoDomainControllerMock
                .Setup(p => p.ProcessarPagamentoMock(identificacao))
                .ReturnsAsync(new ResultadoOperacao());

            var result = await controller.PagamentoMock(identificacao);

            var okResult = Assert.IsType<OkResult>(result);            
        }

         [Fact]
        public async Task PagamentoMockBadRequest()
        {
            var identificacao = Guid.NewGuid();
            pagamentoDomainControllerMock
                .Setup(p => p.ProcessarPagamentoMock(identificacao)).ReturnsAsync(new ResultadoOperacao("Erro"));

            var result = await controller.PagamentoMock(identificacao);


            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<ErrorResponse>(badRequestResult.Value);
        }

        [Fact]
        public async Task PagamentoMockInternalServerError()
        {
            var identificacao = Guid.NewGuid();
            pagamentoDomainControllerMock
                .Setup(p => p.ProcessarPagamentoMock(identificacao))
                .ThrowsAsync(new Exception());

            var result = await controller.PagamentoMock(identificacao);

            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.IsType<ErrorResponse>(internalServerErrorResult.Value);
        }

        [Fact]
        public async Task PagamentoHookOk()
        {
            pagamentoDomainControllerMock
                .Setup(p => p.ProcessarPagamento(It.IsAny<PagamentoProcessadoDto>()))
                .ReturnsAsync(new ResultadoOperacao());

            var result = await controller.PagamentoHook(new PagamentoProcessadoDto());

            var okResult = Assert.IsType<OkResult>(result);            
        }

         [Fact]
        public async Task PagamentoHookBadRequest()
        {
            pagamentoDomainControllerMock
                .Setup(p => p.ProcessarPagamento(It.IsAny<PagamentoProcessadoDto>()))
                .ReturnsAsync(new ResultadoOperacao("Erro"));

            var result = await controller.PagamentoHook(new PagamentoProcessadoDto());


            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<ErrorResponse>(badRequestResult.Value);
        }

        [Fact]
        public async Task PagamentoHookInternalServerError()
        {
            pagamentoDomainControllerMock
                .Setup(p => p.ProcessarPagamento(It.IsAny<PagamentoProcessadoDto>()))
                .ThrowsAsync(new Exception());

            var result = await controller.PagamentoHook(new PagamentoProcessadoDto());

            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.IsType<ErrorResponse>(internalServerErrorResult.Value);
        }

        [Fact]
        public async Task CriarPagamentoParaPedidoOk()
        {
            pagamentoDomainControllerMock
                .Setup(p => p.CriarPagamento(It.IsAny<PedidoDto>()))
                .ReturnsAsync(new ResultadoOperacao<PagamentoDto>(new PagamentoDto()));

            var result = await controller.CriarPagamentoParaPedido(ObjectsBuilder.BuildPedidoDto());

            var okResult = Assert.IsType<OkObjectResult>(result);      
            Assert.IsType<PagamentoDto>(okResult.Value);      
        }

        [Fact]
        public async Task CriarPagamentoParaPedidoRequest()
        {
            pagamentoDomainControllerMock
                .Setup(p => p.CriarPagamento(It.IsAny<PedidoDto>()))
                .ReturnsAsync(new ResultadoOperacao<PagamentoDto>("Erro",true));

            var result = await controller.CriarPagamentoParaPedido(ObjectsBuilder.BuildPedidoDto());


            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<ErrorResponse>(badRequestResult.Value);
        }

        [Fact]
        public async Task CriarPagamentoParaPedidoInternalServerError()
        {
            pagamentoDomainControllerMock
                .Setup(p => p.CriarPagamento(It.IsAny<PedidoDto>()))
                .ThrowsAsync(new Exception());

            var result = await controller.CriarPagamentoParaPedido(ObjectsBuilder.BuildPedidoDto());

            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.IsType<ErrorResponse>(internalServerErrorResult.Value);
        }
    
         [Fact]
        public async Task CriarPagamentoMockOk()
        {
            pagamentoDomainControllerMock
                .Setup(p => p.CriarPagamentoMock(It.IsAny<PedidoDto>()))
                .ReturnsAsync(new ResultadoOperacao<PagamentoDto>(new PagamentoDto()));

            var result = await controller.CriarPagamentoMock(ObjectsBuilder.BuildPedidoDto());

            var okResult = Assert.IsType<OkObjectResult>(result);      
            Assert.IsType<PagamentoDto>(okResult.Value);      
        }

        [Fact]
        public async Task CriarPagamentoMockRequest()
        {
            pagamentoDomainControllerMock
                .Setup(p => p.CriarPagamentoMock(It.IsAny<PedidoDto>()))
                .ReturnsAsync(new ResultadoOperacao<PagamentoDto>("Erro",true));

            var result = await controller.CriarPagamentoMock(ObjectsBuilder.BuildPedidoDto());


            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<ErrorResponse>(badRequestResult.Value);
        }

        [Fact]
        public async Task CriarPagamentoMockInternalServerError()
        {
            pagamentoDomainControllerMock
                .Setup(p => p.CriarPagamentoMock(It.IsAny<PedidoDto>()))
                .ThrowsAsync(new Exception());

            var result = await controller.CriarPagamentoMock(ObjectsBuilder.BuildPedidoDto());

            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.IsType<ErrorResponse>(internalServerErrorResult.Value);
        }
    }
}