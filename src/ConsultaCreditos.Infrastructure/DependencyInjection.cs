using ConsultaCreditos.Application.Handlers;
using ConsultaCreditos.Application.Interfaces;
using ConsultaCreditos.Domain.Interfaces;
using ConsultaCreditos.Infrastructure.Data;
using ConsultaCreditos.Infrastructure.Messaging;
using ConsultaCreditos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsultaCreditos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICreditoRepository, CreditoRepository>();

        services.AddSingleton<IServiceBusPublisher, ServiceBusPublisher>();
        services.AddSingleton<ServiceBusConsumer>();

        services.AddScoped<ProcessarCreditoHandler>();

        return services;
    }
}
