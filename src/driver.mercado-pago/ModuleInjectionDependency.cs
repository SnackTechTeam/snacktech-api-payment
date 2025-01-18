using common.Interfaces;
using driver.mercado_pago.services;
using Microsoft.Extensions.DependencyInjection;

namespace driver.mercado_pago
{
    public static class ModuleInjectionDependency
    {
        public static IServiceCollection AddMercadoPagoService(this IServiceCollection services){

            services.AddTransient<IMercadoPagoIntegration, MercadoPagoService>();

            return services;
        }
    }
}