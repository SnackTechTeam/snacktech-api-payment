using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using common.Interfaces;
using common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace driver.amazon.sqs
{
    public class SqsService: ISqsIntegration
    {
        private readonly ILogger<SqsService> logger;
        private readonly IAmazonSQS sqsClient;
        private readonly string queueUrl;

        public SqsService(ILogger<SqsService> logger, IOptions<SqsOptions> sqsOptions){
            this.logger = logger;
            
            var settings = sqsOptions.Value;
            var config = new AmazonSQSConfig{
                    ServiceURL = settings.ServiceUrl
                };
                
            this.sqsClient = new AmazonSQSClient(settings.AwsAccessKeyId, settings.AwsSecretAccessKey, config);
            queueUrl = settings.QueueUrl;
        }

        public async Task SendMessageAsync<T>(T message){
            var serializedMessage = JsonConvert.SerializeObject(message);
            var request = new SendMessageRequest{
                QueueUrl = queueUrl,
                MessageBody = serializedMessage
            };
            await sqsClient.SendMessageAsync(request);
            logger.LogInformation("Message sended to {QueueUrl}: {SerializedMessage}",queueUrl,serializedMessage);
        }
        
    }
}