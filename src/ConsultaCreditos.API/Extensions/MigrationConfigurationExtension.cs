using ConsultaCreditos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConsultaCreditos.API.Extensions
{
    public static class MigrationConfigurationExtension
    {
        public static void ActiveDBMigration(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Aplicando migrations...");
                    context.Database.Migrate();
                    logger.LogInformation("Migrations aplicadas com sucesso!");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro ao aplicar migrations");
                    throw;
                }
            }
        }
    }
}
