using AutoMapper;
using ConsultaCreditos.API.BackgroundServices;
using ConsultaCreditos.API.Extensions;
using ConsultaCreditos.API.Middlewares;
using ConsultaCreditos.Application.Handlers;
using ConsultaCreditos.Application.Mappings;
using ConsultaCreditos.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiCustomService();

builder.Services.AddInfrastructure(builder.Configuration);

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<CreditoProfile>();
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());

builder.Services.AddScoped<IntegrarCreditoHandler>();
builder.Services.AddScoped<ObterCreditosPorNfseHandler>();
builder.Services.AddScoped<ObterCreditoPorNumeroHandler>();
builder.Services.HealthCheckCustomService
    (
     builder.Configuration.GetConnectionString("DefaultConnection")!,
     builder.Configuration["ServiceBus:ConnectionString"]!,
     builder.Configuration["ServiceBus:TopicName"]!
    );

if (builder.Configuration.GetValue<bool>("ServiceBus:Enabled"))
{
    builder.Services.AddHostedService<CreditoProcessorService>();
}

var app = builder.Build();

app.ActiveDBMigration();

app.SwaggerCustomConfig();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.HealthCheckCustomConfig();

if (!app.Environment.EnvironmentName.Equals("Testing", StringComparison.OrdinalIgnoreCase))
{
    Log.Information("{ApplicationName} iniciada com observabilidade completa em {EnvironmentName}", app.Environment.ApplicationName, app.Environment.EnvironmentName);
}

Environment.SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1");
Environment.SetEnvironmentVariable("DOTNET_SKIP_FIRST_TIME_EXPERIENCE", "1");

app.Run();
