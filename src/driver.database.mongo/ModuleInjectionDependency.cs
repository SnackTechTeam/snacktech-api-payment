
using common.Interfaces;
using driver.database.mongo.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace driver.database.mongo
{
    public static class ModuleInjectionDependency
    {
        public static IServiceCollection AddMongoDbService(this IServiceCollection services){
            services.AddTransient<IMongoDbIntegration,PagamentoRepository>();
            return services;
        }
    }
}