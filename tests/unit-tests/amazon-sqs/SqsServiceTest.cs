using Amazon.SQS;
using Amazon.SQS.Model;
using common.Options;
using driver.amazon.sqs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;

namespace unit_tests.amazonsqs
{
    public class SqsServiceTest
    {
        [Fact]
        public async Task SendMessageAsyncEnviarMensagemComSucesso()
        {
            var loggerMock = new Mock<ILogger<SqsService>>();
            var sqsClientMock = new Mock<IAmazonSQS>();
            var sqsOptions = Options.Create(new SqsOptions
            {
                AwsAccessKeyId = "access-key",
                AwsSecretAccessKey = "secret-key",
                AwsSecretAccessToken = "token",
                ServiceUrl = "http://localhost",
                QueueUrl = "http://localhost/fila"
            });

            var sqsService = new SqsService(loggerMock.Object,sqsClientMock.Object, sqsOptions);
            var testMessage = new { Name = "Exemplo" };
            var serializedMessage = JsonConvert.SerializeObject(testMessage);

            sqsClientMock.Setup(client => client.SendMessageAsync(It.Is<SendMessageRequest>(req =>
                req.QueueUrl == sqsOptions.Value.QueueUrl &&
                req.MessageBody == serializedMessage), default))
                .ReturnsAsync(new SendMessageResponse());

            await sqsService.SendMessageAsync(testMessage);

            sqsClientMock.Verify(client => client.SendMessageAsync(It.Is<SendMessageRequest>(req =>
                req.QueueUrl == sqsOptions.Value.QueueUrl &&
                req.MessageBody == serializedMessage), default), Times.Once);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            loggerMock.Verify(
                    x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Message sended to {sqsOptions.Value.QueueUrl}")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}