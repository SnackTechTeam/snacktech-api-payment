using common.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Configuration
{
    public static class MongoDbExtensions
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services){

            services.AddSingleton<IMongoClient>(sp => {
                var settings = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;

                string connectionString = "";

                if(settings.ConnectionString != null){
                    connectionString = settings.ConnectionString;
                } else {
                    var connectionStringBase = $"mongodb://{settings.UserName}:{settings.Password}@{settings.Endpoint}:{settings.Port}";
                    connectionString = settings.SSL ? $"{connectionStringBase}/?ssl=true&sslCAFile={settings.SslCertificatePath}":connectionStringBase;
                }

                return new MongoClient(connectionString);
            });

            services.AddScoped<IMongoDatabase>(sp => {
                var client = sp.GetRequiredService<IMongoClient>();
                var settings = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                return client.GetDatabase(settings.DatabaseName);
            });

            return services;
        }        
    }
}