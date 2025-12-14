using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Enums;
using ConsultaCreditos.Domain.Exceptions;
using FluentAssertions;

namespace ConsultaCreditos.UnitTests.Domain.Entities;

public class CreditoTests
{
    [Fact]
    public void Criar_ComDadosValidos_DeveCriarCredito()
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

        credito.Should().NotBeNull();
        credito.NumeroCredito.Should().Be("123456");
        credito.NumeroNfse.Should().Be("7891011");
        credito.DataConstituicao.Should().Be(new DateTime(2024, 2, 25));
        credito.ValorIssqn.Should().Be(1250m);
        credito.TipoCredito.Should().Be(TipoCredito.ISSQN);
        credito.SimplesNacional.Should().BeTrue();
        credito.Aliquota.Should().Be(5m);
        credito.ValorFaturado.Should().Be(30000m);
        credito.ValorDeducao.Should().Be(5000m);
        credito.BaseCalculo.Should().Be(25000m);
    }

    [Fact]
    public void Criar_ComDataConstituicaoFutura_DeveLancarExcecao()
    {
        var dataFutura = DateTime.Now.AddDays(1);

        var act = () => Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "7891011",
            dataConstituicao: dataFutura,
            valorIssqn: 1250m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: 5m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 25000m
        );

        act.Should().Throw<CreditoInvalidoException>()
            .WithMessage("*não pode ser futura*");
    }

    [Fact]
    public void Criar_ComValorIssqnZero_DeveLancarExcecao()
    {
        var act = () => Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 25),
            valorIssqn: 0m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: 5m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 25000m
        );

        act.Should().Throw<CreditoInvalidoException>()
            .WithMessage("*deve ser maior que zero*");
    }

    [Fact]
    public void Criar_ComBaseCalculoInvalida_DeveLancarExcecao()
    {
        var act = () => Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 25),
            valorIssqn: 1250m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: 5m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 20000m
        );

        act.Should().Throw<CreditoInvalidoException>()
            .WithMessage("*Base de cálculo inválida*");
    }

    [Fact]
    public void Criar_ComValorIssqnInvalido_DeveLancarExcecao()
    {
        var act = () => Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 25),
            valorIssqn: 1000m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: 5m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 25000m
        );

        act.Should().Throw<CreditoInvalidoException>()
            .WithMessage("*Valor do ISSQN inválido*");
    }

    [Fact]
    public void Criar_ComNumeroCreditoVazio_DeveLancarExcecao()
    {
        var act = () => Credito.Criar(
            numeroCredito: "",
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

        act.Should().Throw<CreditoInvalidoException>()
            .WithMessage("*obrigatório*");
    }

    [Fact]
    public void Criar_ComNumeroNfseVazio_DeveLancarExcecao()
    {
        var act = () => Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "",
            dataConstituicao: new DateTime(2024, 2, 25),
            valorIssqn: 1250m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: 5m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 25000m
        );

        act.Should().Throw<CreditoInvalidoException>()
            .WithMessage("*obrigatório*");
    }

    [Fact]
    public void Criar_ComAliquotaNegativa_DeveLancarExcecao()
    {
        var act = () => Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 25),
            valorIssqn: 1250m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: -5m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 25000m
        );

        act.Should().Throw<CreditoInvalidoException>()
            .WithMessage("*entre 0 e 100*");
    }

    [Fact]
    public void Criar_ComAliquotaMaiorQue100_DeveLancarExcecao()
    {
        var act = () => Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 25),
            valorIssqn: 1250m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: 101m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 25000m
        );

        act.Should().Throw<CreditoInvalidoException>()
            .WithMessage("*entre 0 e 100*");
    }

    [Fact]
    public void Criar_ComTipoCreditoOutros_DeveCriarCredito()
    {
        var credito = Credito.Criar(
            numeroCredito: "654321",
            numeroNfse: "1122334",
            dataConstituicao: new DateTime(2024, 1, 15),
            valorIssqn: 595m,
            tipoCredito: TipoCredito.Outros,
            simplesNacional: true,
            aliquota: 3.5m,
            valorFaturado: 20000m,
            valorDeducao: 3000m,
            baseCalculo: 17000m
        );

        credito.Should().NotBeNull();
        credito.TipoCredito.Should().Be(TipoCredito.Outros);
    }

    [Fact]
    public void Criar_ComSimplesNacionalFalso_DeveCriarCredito()
    {
        var credito = Credito.Criar(
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

        credito.Should().NotBeNull();
        credito.SimplesNacional.Should().BeFalse();
    }
}
