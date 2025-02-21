using System.Diagnostics.CodeAnalysis;
using api.Configuration;
using api.Configuration.HealthChecks;
using common.Options;
using driver.amazon.sqs;
using driver.database.mongo;
using driver.mercado_pago;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

builder.Services.Configure<MercadoPagoOptions>(builder.Configuration.GetSection("MercadoPagoOptions"));
builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDbOptions"));
builder.Services.Configure<SqsOptions>(builder.Configuration.GetSection("SqsOptions"));

// Add services to the container.
builder.Services.AddAmazonSqs();
builder.Services.AddSqsService();
builder.Services.AddMongoDB();
builder.Services.AddMongoDbService();
builder.Services.AddHttpClient();
builder.Services.AddMercadoPagoService();
builder.Services.AddDomainControllers();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddSwaggerGen(c =>{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SnackTech Payment API", Version = "v1" });
});

builder.Services.AddCustomHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SnackTech Payment API v1");
});


app.UseCustomHealthChecks();
app.UseAuthorization();

// Redirecionamento da URL raiz para /swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.MapControllers();

await app.RunAsync();

[ExcludeFromCodeCoverage]
public static partial class Program { }
