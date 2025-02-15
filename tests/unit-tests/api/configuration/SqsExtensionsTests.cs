

using Amazon.SQS;
using api.Configuration;
using common.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace unit_tests.api.configuration
{
    public class SqsExtensionsTests
    {
        [Fact]
        public void AddAmazonSqs_ShouldRegisterIAmazonSQS()
        {
            var sqsOptions = new SqsOptions
            {
                ServiceUrl = "http://localhost:4566",
                AwsAccessKeyId = "testAccessKeyId",
                AwsSecretAccessKey = "testSecretAccessKey",
                AwsSecretAccessToken = "testSecretAccessToken"
            };

            var optionsMock = new Mock<IOptions<SqsOptions>>();
            optionsMock.Setup(o => o.Value).Returns(sqsOptions);

            var services = new ServiceCollection();
            services.AddSingleton(optionsMock.Object);
            services.AddAmazonSqs();

            var serviceProvider = services.BuildServiceProvider();

            var amazonSqs = serviceProvider.GetService<IAmazonSQS>();

            Assert.NotNull(amazonSqs);
            Assert.IsType<AmazonSQSClient>(amazonSqs);

            var amazonSqsClient = amazonSqs as AmazonSQSClient;
            Assert.Contains(sqsOptions.ServiceUrl, amazonSqsClient?.Config.ServiceURL);
        }
    }
}