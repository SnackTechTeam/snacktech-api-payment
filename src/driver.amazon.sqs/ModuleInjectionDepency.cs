using common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace driver.amazon.sqs
{
    public static class ModuleInjectionDepency
    {
        public static IServiceCollection AddSqsService(this IServiceCollection services)
        {
            services.AddTransient<ISqsIntegration,SqsService>();
            return services;
        }

    }
}