using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Enums;
using ConsultaCreditos.Infrastructure.Data;
using ConsultaCreditos.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ConsultaCreditos.UnitTests.Infrastructure.Repositories;

public class CreditoRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly CreditoRepository _repository;

    public CreditoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new CreditoRepository(_context);
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarCreditoNoBanco()
    {
        var credito = Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 25),
            valorIssqn: 1250m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: 5m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 25000m
        );

        await _repository.AdicionarAsync(credito);

        var creditoNoBanco = await _context.Creditos.FirstOrDefaultAsync();
        creditoNoBanco.Should().NotBeNull();
        creditoNoBanco!.NumeroCredito.Should().Be("123456");
    }

    [Fact]
    public async Task ObterPorNumeroNfseAsync_ComCreditosExistentes_DeveRetornarLista()
    {
        var credito1 = Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 25),
            valorIssqn: 1250m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: 5m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 25000m
        );

        var credito2 = Credito.Criar(
            numeroCredito: "789012",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 26),
            valorIssqn: 945m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: false,
            aliquota: 4.5m,
            valorFaturado: 25000m,
            valorDeducao: 4000m,
            baseCalculo: 21000m
        );

        await _repository.AdicionarAsync(credito1);
        await _repository.AdicionarAsync(credito2);

        var result = await _repository.ObterPorNumeroNfseAsync("7891011");

        result.Should().HaveCount(2);
        result[0].NumeroCredito.Should().Be("789012");
        result[1].NumeroCredito.Should().Be("123456");
    }

    [Fact]
    public async Task ObterPorNumeroNfseAsync_SemCreditosExistentes_DeveRetornarListaVazia()
    {
        var result = await _repository.ObterPorNumeroNfseAsync("9999999");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ObterPorNumeroCreditoAsync_ComCreditoExistente_DeveRetornarCredito()
    {
        var credito = Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 25),
            valorIssqn: 1250m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: 5m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 25000m
        );

        await _repository.AdicionarAsync(credito);

        var result = await _repository.ObterPorNumeroCreditoAsync("123456");

        result.Should().NotBeNull();
        result!.NumeroCredito.Should().Be("123456");
    }

    [Fact]
    public async Task ObterPorNumeroCreditoAsync_SemCreditoExistente_DeveRetornarNull()
    {
        var result = await _repository.ObterPorNumeroCreditoAsync("999999");

        result.Should().BeNull();
    }

    [Fact]
    public async Task ExisteAsync_ComCreditoExistente_DeveRetornarTrue()
    {
        var credito = Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 25),
            valorIssqn: 1250m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: 5m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 25000m
        );

        await _repository.AdicionarAsync(credito);

        var result = await _repository.ExisteAsync("123456");

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExisteAsync_SemCreditoExistente_DeveRetornarFalse()
    {
        var result = await _repository.ExisteAsync("999999");

        result.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
