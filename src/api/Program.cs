using System.Reflection;
using api.Configuration;
using api.Configuration.HealthChecks;
using common.Options;
using driver.mercado_pago;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//TODO: Recuperar valores de configuração por variável de ambiente ao invés do appsettings
builder.Services.Configure<MercadoPagoOptions>(builder.Configuration.GetSection("MercadoPagoOptions"));
builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDbOptions"));
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMqOptions"));
builder.Services.Configure<RabbitMqPublishValues>(builder.Configuration.GetSection("RabbitMqPublishValues"));

// Add services to the container.
builder.Services.AddRabbitMQ();
builder.Services.AddMongoDB();
builder.Services.AddHttpClient();
builder.Services.AddMercadoPagoService();
builder.Services.AddDomainControllers();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddSwaggerGen(c =>{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SnackTech Payment API", Version = "v1" });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
