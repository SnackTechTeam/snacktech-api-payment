using common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace driver.rabbitmq
{
    public class RabbitConnection : IDisposable
    {
        private readonly IConnectionFactory connectionFactory;
        bool disposed = false;
        readonly object sync_root = new object();
        IConnection? connection;

        private readonly ILogger<RabbitConnection> logger;

        public RabbitConnection(ILogger<RabbitConnection> logger, IOptions<RabbitMqOptions> rabbitOptions){
            this.logger = logger;
            var rabbitMqOptions = rabbitOptions.Value;
            this.connectionFactory = new ConnectionFactory{
                HostName = rabbitMqOptions.HostName,
                UserName = rabbitMqOptions.UserName,
                Password = rabbitMqOptions.Password,
                Port = rabbitMqOptions.Port
            };
        }

        public async Task InitializeAsync(){
            this.connection = await connectionFactory.CreateConnectionAsync();
        }

        public bool IsConnected 
        {
            get
            {
                return connection != null && connection.IsOpen && !disposed;
            }
        }

        public async Task<IChannel> CreateChannel()
        {
            if(!IsConnected)
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");

            return await connection!.CreateChannelAsync();
        }

        public async Task<bool> TryConnect()
        {
            logger.LogInformation("RabbitMQ Client is trying to connect...");
            
            await InitializeAsync();
            lock(sync_root){
                if(connection is null)
                {
                    logger.LogCritical("FATAL ERROR: RabbitMQ connection is null during attempt of connection");
                    return false;
                }
                if(IsConnected){
                    connection.ConnectionShutdownAsync += OnConnectionShutdownAsync;
                    connection.CallbackExceptionAsync += OnCallbackExceptionAsync;
                    connection.ConnectionBlockedAsync += OnConnectionBlockedAsync;
                    return true;
                }
                else
                {
                    logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                    return false;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if(disposing){
                try{
                    connection!.Dispose();
                }
                catch(IOException ex){
                    logger.LogCritical(ex.ToString());
                }
            }

            disposed = true;
        }

        public void Dispose(){
            Dispose(true);
            GC.SuppressFinalize(this);
        }

         private async Task OnConnectionBlockedAsync(object? sender, ConnectionBlockedEventArgs e){
            if(disposed) return;

            logger.LogWarning("RabbitMQ connection is shutdown. Trying to reconnect...");

            await TryConnect();
        }

        private async Task OnCallbackExceptionAsync(object? sender, CallbackExceptionEventArgs e){
            if (disposed) return;

            logger.LogWarning("RabbitMQ connection throw exception. Trying to reconnect...");

            await TryConnect();
        }

        private async Task OnConnectionShutdownAsync(object? sender, ShutdownEventArgs reason){
            if (disposed) return;

            logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to reconnect...");

            await TryConnect();
        }

        ~RabbitConnection()
        {
            Dispose(false);
        }
    }
}