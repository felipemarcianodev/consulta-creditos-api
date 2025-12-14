using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Enums;
using ConsultaCreditos.Domain.Exceptions;
using ConsultaCreditos.Domain.Services;
using FluentAssertions;

namespace ConsultaCreditos.UnitTests.Domain.Services;

public class CreditoValidationServiceTests
{
    private readonly CreditoValidationService _validationService;

    public CreditoValidationServiceTests()
    {
        _validationService = new CreditoValidationService();
    }

    [Fact]
    public void ValidarBaseCalculo_ComValoresCorretos_NaoDeveLancarExcecao()
    {
        var valorFaturado = 30000m;
        var valorDeducao = 5000m;
        var baseCalculo = 25000m;

        var act = () => _validationService.ValidarBaseCalculo(valorFaturado, valorDeducao, baseCalculo);

        act.Should().NotThrow();
    }

    [Fact]
    public void ValidarBaseCalculo_ComValorIncorreto_DeveLancarExcecao()
    {
        var valorFaturado = 30000m;
        var valorDeducao = 5000m;
        var baseCalculo = 20000m;

        var act = () => _validationService.ValidarBaseCalculo(valorFaturado, valorDeducao, baseCalculo);

        act.Should().Throw<CreditoInvalidoException>()
            .WithMessage("*Base de cálculo inválida*");
    }

    [Fact]
    public void ValidarValorIssqn_ComValoresCorretos_NaoDeveLancarExcecao()
    {
        var baseCalculo = 25000m;
        var aliquota = 5m;
        var valorIssqn = 1250m;

        var act = () => _validationService.ValidarValorIssqn(baseCalculo, aliquota, valorIssqn);

        act.Should().NotThrow();
    }

    [Fact]
    public void ValidarValorIssqn_ComValorIncorreto_DeveLancarExcecao()
    {
        var baseCalculo = 25000m;
        var aliquota = 5m;
        var valorIssqn = 1000m;

        var act = () => _validationService.ValidarValorIssqn(baseCalculo, aliquota, valorIssqn);

        act.Should().Throw<CreditoInvalidoException>()
            .WithMessage("*Valor do ISSQN inválido*");
    }

    [Fact]
    public void ValidarCalculos_ComCreditoValido_NaoDeveLancarExcecao()
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

        var act = () => _validationService.ValidarCalculos(credito);

        act.Should().NotThrow();
    }

    [Fact]
    public void ValidarConsistencia_ComCreditoValido_NaoDeveLancarExcecao()
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

        var act = () => _validationService.ValidarConsistencia(credito);

        act.Should().NotThrow();
    }

    [Fact]
    public void ValidarBaseCalculo_ComValoresCorretosPrecisao_NaoDeveLancarExcecao()
    {
        var valorFaturado = 30000.50m;
        var valorDeducao = 5000.25m;
        var baseCalculo = 25000.25m;

        var act = () => _validationService.ValidarBaseCalculo(valorFaturado, valorDeducao, baseCalculo);

        act.Should().NotThrow();
    }

    [Fact]
    public void ValidarValorIssqn_ComValoresCorretosPrecisao_NaoDeveLancarExcecao()
    {
        var baseCalculo = 25000.50m;
        var aliquota = 5.5m;
        var valorIssqn = 1375.03m;

        var act = () => _validationService.ValidarValorIssqn(baseCalculo, aliquota, valorIssqn);

        act.Should().NotThrow();
    }
}
