using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Validators;
using FluentAssertions;

namespace ConsultaCreditos.UnitTests.Application.Validators;

public class IntegrarCreditoRequestValidatorTests
{
    private readonly IntegrarCreditoRequestValidator _validator;

    public IntegrarCreditoRequestValidatorTests()
    {
        _validator = new IntegrarCreditoRequestValidator();
    }

    [Fact]
    public async Task Validate_ComDadosValidos_DeveRetornarSucesso()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "123456",
            NumeroNfse = "7891011",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 1250m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Sim",
            Aliquota = 5m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_ComNumeroCreditoVazio_DeveRetornarErro()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "",
            NumeroNfse = "7891011",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 1250m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Sim",
            Aliquota = 5m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "NumeroCredito");
    }

    [Fact]
    public async Task Validate_ComNumeroNfseVazio_DeveRetornarErro()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "123456",
            NumeroNfse = "",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 1250m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Sim",
            Aliquota = 5m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "NumeroNfse");
    }

    [Fact]
    public async Task Validate_ComDataConstituicaoFutura_DeveRetornarErro()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "123456",
            NumeroNfse = "7891011",
            DataConstituicao = DateTime.Now.AddDays(1),
            ValorIssqn = 1250m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Sim",
            Aliquota = 5m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DataConstituicao");
    }

    [Fact]
    public async Task Validate_ComValorIssqnZero_DeveRetornarErro()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "123456",
            NumeroNfse = "7891011",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 0m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Sim",
            Aliquota = 5m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ValorIssqn");
    }

    [Fact]
    public async Task Validate_ComTipoCreditoInvalido_DeveRetornarErro()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "123456",
            NumeroNfse = "7891011",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 1250m,
            TipoCredito = "INVALIDO",
            SimplesNacional = "Sim",
            Aliquota = 5m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "TipoCredito");
    }

    [Fact]
    public async Task Validate_ComSimplesNacionalInvalido_DeveRetornarErro()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "123456",
            NumeroNfse = "7891011",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 1250m,
            TipoCredito = "ISSQN",
            SimplesNacional = "INVALIDO",
            Aliquota = 5m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "SimplesNacional");
    }

    [Fact]
    public async Task Validate_ComAliquotaNegativa_DeveRetornarErro()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "123456",
            NumeroNfse = "7891011",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 1250m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Sim",
            Aliquota = -5m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Aliquota");
    }

    [Fact]
    public async Task Validate_ComAliquotaMaiorQue100_DeveRetornarErro()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "123456",
            NumeroNfse = "7891011",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 1250m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Sim",
            Aliquota = 101m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Aliquota");
    }

    [Fact]
    public async Task Validate_ComBaseCalculoInvalida_DeveRetornarErro()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "123456",
            NumeroNfse = "7891011",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 1250m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Sim",
            Aliquota = 5m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 20000m
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_ComValorIssqnInvalido_DeveRetornarErro()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "123456",
            NumeroNfse = "7891011",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 1000m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Sim",
            Aliquota = 5m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
    }
}
