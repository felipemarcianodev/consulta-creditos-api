using AutoMapper;
using ConsultaCreditos.API.BackgroundServices;
using ConsultaCreditos.API.Middlewares;
using ConsultaCreditos.Application.Handlers;
using ConsultaCreditos.Application.Mappings;
using ConsultaCreditos.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "API de Consulta de Créditos Constituídos",
            Version = "v1",
            Description = "API RESTful para integração e consulta de créditos constituídos com processamento assíncrono via Azure Service Bus",
            Contact = new()
            {
                Name = "Equipe de Desenvolvimento",
                Email = "contato@example.com"
            }
        };
        return Task.CompletedTask;
    });
});

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
    .AddCheck(
        "self",
        () => HealthCheckResult.Healthy(),
        tags: new[] { "self" })

    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "postgresql",
        tags: new[] { "db", "ready" })

    .AddAzureServiceBusTopic(
        builder.Configuration["ServiceBus:ConnectionString"]!,
        builder.Configuration["ServiceBus:TopicName"]!,
        name: "servicebus",
        tags: new[] { "messaging", "ready" });

if (builder.Configuration.GetValue<bool>("ServiceBus:Enabled"))
{
    builder.Services.AddHostedService<CreditoProcessorService>();
}


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API de Consulta de Créditos v1");
        options.DocumentTitle = "API de Consulta de Créditos - Documentação";
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health/self", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

Log.Information("Iniciando ConsultaCreditos.API");

app.Run();
