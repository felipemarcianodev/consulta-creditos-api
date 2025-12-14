using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ConsultaCreditos.API.Extensions
{
    public static class HealthCheckConfigurationExtension
    {
        public static void HealthCheckCustomService(this IServiceCollection services, 
            string connectionStringPostgres,
            string connectionStringServiceBus,
            string topicServiceBus)
        {
            services.AddHealthChecks()
            .AddCheck(
                "self",
                () => HealthCheckResult.Healthy(),
                tags: new[] { "self" })

            .AddNpgSql(
               connectionStringPostgres,
                name: "postgresql",
                tags: new[] { "db", "ready" })

            .AddAzureServiceBusTopic(
                connectionStringServiceBus,
                topicServiceBus,
                name: "servicebus",
                tags: new[] { "messaging", "ready" });
        }

        public static void HealthCheckCustomConfig(this WebApplication app)
        {
            app.MapHealthChecks("/health/self", new HealthCheckOptions
            {
                Predicate = _ => false
            });

            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready")
            });
        }
    }
}
