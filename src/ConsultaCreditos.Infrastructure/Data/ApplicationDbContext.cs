using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ConsultaCreditos.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Credito> Creditos => Set<Credito>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CreditoConfiguration());
    }
}
