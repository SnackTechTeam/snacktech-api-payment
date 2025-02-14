
namespace common.Interfaces
{
    public interface IRabbitIntegration
    {
        Task Publish<T>(string exchangeName, string routeKey, T message);
    }
}