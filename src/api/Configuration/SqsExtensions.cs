using Amazon.SQS;
using common.Options;
using Microsoft.Extensions.Options;

namespace api.Configuration
{
    public static class SqsExtensions
    {
        public static IServiceCollection AddAmazonSqs(this IServiceCollection services)
        {
            services.AddTransient<IAmazonSQS>(sp =>{
                var settings = sp.GetRequiredService<IOptions<SqsOptions>>().Value;
                var config = new AmazonSQSConfig{
                    ServiceURL = settings.ServiceUrl
                };
                return new AmazonSQSClient(settings.AwsAccessKeyId, settings.AwsSecretAccessKey, config);
            });
            return services;
        }
    }
}