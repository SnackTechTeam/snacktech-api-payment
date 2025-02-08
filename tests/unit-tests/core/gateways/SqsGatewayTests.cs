using common.ExternalSource.Sqs;
using common.Interfaces;
using core.gateways;
using Moq;

namespace unit_tests.core.gateways
{
    public class SqsGatewayTests
    {
        private readonly Mock<ISqsIntegration> _sqsIntegrationMock;
        private readonly SqsGateway _sqsGateway;

        public SqsGatewayTests()
        {
            _sqsIntegrationMock = new Mock<ISqsIntegration>();
            _sqsGateway = new SqsGateway(_sqsIntegrationMock.Object);
        }

        [Fact]
        public async Task PublicarMensagemPagamentoNoSqs_ShouldCallSendMessageAsync()
        {
            // Arrange
            var pagamentoMessageDto = new PagamentoMessageDto();

            _sqsIntegrationMock.Setup(sqs => sqs.SendMessageAsync(pagamentoMessageDto))
                .Returns(Task.CompletedTask);

            // Act
            await _sqsGateway.PublicarMensagemPagamentoNoSqs(pagamentoMessageDto);

            // Assert
            _sqsIntegrationMock.Verify(sqs => sqs.SendMessageAsync(pagamentoMessageDto), Times.Once);
        }
    }
}