using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsultaCreditos.Infrastructure.Data.Configurations;

public class CreditoConfiguration : IEntityTypeConfiguration<Credito>
{
    public void Configure(EntityTypeBuilder<Credito> builder)
    {
        builder.ToTable("credito");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.NumeroCredito)
            .HasColumnName("numero_credito")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.NumeroNfse)
            .HasColumnName("numero_nfse")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.DataConstituicao)
            .HasColumnName("data_constituicao")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(c => c.ValorIssqn)
            .HasColumnName("valor_issqn")
            .HasColumnType("decimal(15,2)")
            .IsRequired();

        builder.Property(c => c.TipoCredito)
            .HasColumnName("tipo_credito")
            .HasMaxLength(50)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<TipoCredito>(v));

        builder.Property(c => c.SimplesNacional)
            .HasColumnName("simples_nacional")
            .IsRequired();

        builder.Property(c => c.Aliquota)
            .HasColumnName("aliquota")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(c => c.ValorFaturado)
            .HasColumnName("valor_faturado")
            .HasColumnType("decimal(15,2)")
            .IsRequired();

        builder.Property(c => c.ValorDeducao)
            .HasColumnName("valor_deducao")
            .HasColumnType("decimal(15,2)")
            .IsRequired();

        builder.Property(c => c.BaseCalculo)
            .HasColumnName("base_calculo")
            .HasColumnType("decimal(15,2)")
            .IsRequired();

        builder.HasIndex(c => c.NumeroCredito)
            .IsUnique()
            .HasDatabaseName("ix_credito_numero_credito");

        builder.HasIndex(c => c.NumeroNfse)
            .HasDatabaseName("ix_credito_numero_nfse");
    }
}
