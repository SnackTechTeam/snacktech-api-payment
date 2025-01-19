using core.controllers;
using core.interfaces;

namespace api.Configuration
{
    public static class DomainInjectionDependency
    {
        public static IServiceCollection AddDomainControllers(this IServiceCollection services)
        {
            services.AddTransient<IPagamentoController, PagamentoController>();

            return services;
        }
    }
}