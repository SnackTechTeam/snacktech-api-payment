using common.Interfaces;
using common.Options;
using driver.rabbitmq;
using Microsoft.Extensions.Options;

namespace api.Configuration
{
    public static class RabbitMqExtensions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services){
            
            services.AddSingleton(sp => {
                var settings = sp.GetRequiredService<IOptions<RabbitMqOptions>>();
                var logger = sp.GetRequiredService<ILogger<RabbitConnection>>();
                var connection = new RabbitConnection(logger,settings);
                return connection;
            });

            services.AddSingleton<IRabbitIntegration>(sp => {
                 var logger = sp.GetRequiredService<ILogger<RabbitService>>();
                 var manager = sp.GetRequiredService<RabbitConnection>();
                 return new RabbitService(logger,manager);
            });
            return services;
        }
    }
}