using System.Net;
using System.Net.Mime;
using System.Text.Json;
using common.Options;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Configuration.HealthChecks
{
    public static class HealthChecksExtensions
    {
        public static  IServiceCollection AddCustomHealthChecks(this IServiceCollection services){
            var sp = services.BuildServiceProvider();
            //MP
            var mpData = sp.GetRequiredService<IOptions<MercadoPagoOptions>>().Value;
            AddMercadoPagoHc(services,mpData);
                    
            //MongoDB
            services.AddHealthChecks()
                .AddMongoDb((sp) => sp.GetRequiredService<IMongoDatabase>(),"Mongo DB",HealthStatus.Unhealthy);
                
            //SQS

            return services;
        }

        private static void AddMercadoPagoHc(IServiceCollection services, MercadoPagoOptions mercadoPagoOptions){
            services.AddHealthChecks().AddAsyncCheck("Mercado Pago API", async () => {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{mercadoPagoOptions.UrlBase}users/me");
                return response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Forbidden
                    ? HealthCheckResult.Healthy("Mercado Pago API está funcionando")
                    : HealthCheckResult.Unhealthy("Erro ao acessar a API do Mercado Pago");
            }, tags: new[] { "external", "mercadopago" });
        }

        public static void UseCustomHealthChecks(this WebApplication app){
            app.MapHealthChecks("api/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    var result = JsonSerializer.Serialize(
                        new
                        {
                            status = report.Status.ToString(),
                            check = report.Entries.Select(entry => new {
                                name = entry.Key,
                                status = entry.Value.Status.ToString(),
                                exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                duration = entry.Value.Duration.ToString()
                            })
                        }
                    );

                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }
            });

            app.MapHealthChecks("api/health/live", new HealthCheckOptions
            {
                Predicate = (_) => false
            });
        }
    }
}