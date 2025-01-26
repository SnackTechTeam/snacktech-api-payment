using System.Text;
using common.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace driver.rabbitmq
{
    public class RabbitService : IRabbitIntegration
    {
        private readonly RabbitConnection rabbitConnection;
        private readonly ILogger<RabbitService> logger;

        public RabbitService(ILogger<RabbitService> logger, RabbitConnection rabbitConnection){
            this.rabbitConnection = rabbitConnection;
            this.logger = logger;
        }

        public async Task Publish<T>(string exchangeName, string routeKey, T message){
            if(!rabbitConnection.IsConnected)
                await rabbitConnection.TryConnect();

            using(var channel = await rabbitConnection.CreateChannel()){
                logger.LogInformation($"Declaring exchange: {exchangeName}...");
                await channel.ExchangeDeclareAsync(exchangeName,"topic",true,false);
                var serializedMessage = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(serializedMessage);
                logger.LogInformation($"Sending message to exchange: {exchangeName}...");
                await channel.BasicPublishAsync(
                        exchange: exchangeName,
                        routingKey: routeKey,
                        body: body);
                logger.LogInformation($"Message sended to exchange: {exchangeName}...");
            }
        }
    }
}