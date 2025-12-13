using AutoMapper;
using ConsultaCreditos.API.BackgroundServices;
using ConsultaCreditos.API.Middlewares;
using ConsultaCreditos.Application.Handlers;
using ConsultaCreditos.Application.Mappings;
using ConsultaCreditos.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<CreditoProfile>();
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());

builder.Services.AddScoped<IntegrarCreditoHandler>();
builder.Services.AddScoped<ObterCreditosPorNfseHandler>();
builder.Services.AddScoped<ObterCreditoPorNumeroHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "postgresql",
        tags: new[] { "db", "ready" })
    .AddAzureServiceBusTopic(
        builder.Configuration["ServiceBus:ConnectionString"]!,
        builder.Configuration["ServiceBus:TopicName"]!,
        name: "servicebus",
        tags: new[] { "messaging", "ready" });

builder.Services.AddHostedService<CreditoProcessorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/self", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

Log.Information("Iniciando ConsultaCreditos.API");

app.Run();
