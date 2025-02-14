
namespace common.Interfaces
{
    public interface ISqsIntegration
    {
        Task SendMessageAsync<T>(T message);
    }
}