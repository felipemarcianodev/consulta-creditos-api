using ConsultaCreditos.Domain.ValueObjects;
using FluentAssertions;

namespace ConsultaCreditos.UnitTests.Domain.ValueObjects;

public class PercentualTests
{
    [Fact]
    public void Criar_ComValorValido_DeveCriarPercentual()
    {
        var valor = 5.0m;

        var percentual = Percentual.Criar(valor);

        percentual.Valor.Should().Be(valor);
    }

    [Fact]
    public void Criar_ComValorComMaisDeDuasCasasDecimais_DeveArredondar()
    {
        var valor = 5.759m;

        var percentual = Percentual.Criar(valor);

        percentual.Valor.Should().Be(5.76m);
    }

    [Fact]
    public void Criar_ComValorNegativo_DeveLancarExcecao()
    {
        var valor = -5m;

        var act = () => Percentual.Criar(valor);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*deve estar entre 0 e 100*");
    }

    [Fact]
    public void Criar_ComValorMaiorQue100_DeveLancarExcecao()
    {
        var valor = 101m;

        var act = () => Percentual.Criar(valor);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*deve estar entre 0 e 100*");
    }

    [Fact]
    public void Criar_ComValorZero_DeveCriarPercentual()
    {
        var percentual = Percentual.Criar(0);

        percentual.Valor.Should().Be(0);
    }

    [Fact]
    public void Criar_ComValor100_DeveCriarPercentual()
    {
        var percentual = Percentual.Criar(100);

        percentual.Valor.Should().Be(100);
    }

    [Fact]
    public void Zero_DeveRetornarPercentualComValorZero()
    {
        var zero = Percentual.Zero;

        zero.Valor.Should().Be(0);
    }

    [Fact]
    public void ParaDecimal_DeveRetornarValorDividoPor100()
    {
        var percentual = Percentual.Criar(5m);

        var resultado = percentual.ParaDecimal();

        resultado.Should().Be(0.05m);
    }

    [Fact]
    public void AplicarSobre_DeveRetornarValorCorreto()
    {
        var percentual = Percentual.Criar(5m);
        var valor = Dinheiro.Criar(1000m);

        var resultado = percentual.AplicarSobre(valor);

        resultado.Valor.Should().Be(50m);
    }

    [Fact]
    public void Equals_ComMesmoValor_DeveRetornarTrue()
    {
        var percentual1 = Percentual.Criar(5m);
        var percentual2 = Percentual.Criar(5m);

        percentual1.Equals(percentual2).Should().BeTrue();
        (percentual1 == percentual2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ComValoresDiferentes_DeveRetornarFalse()
    {
        var percentual1 = Percentual.Criar(5m);
        var percentual2 = Percentual.Criar(10m);

        percentual1.Equals(percentual2).Should().BeFalse();
        (percentual1 != percentual2).Should().BeTrue();
    }

    [Fact]
    public void OperadorMaiorQue_DeveRetornarResultadoCorreto()
    {
        var percentual1 = Percentual.Criar(10m);
        var percentual2 = Percentual.Criar(5m);

        (percentual1 > percentual2).Should().BeTrue();
        (percentual2 > percentual1).Should().BeFalse();
    }

    [Fact]
    public void OperadorMenorQue_DeveRetornarResultadoCorreto()
    {
        var percentual1 = Percentual.Criar(5m);
        var percentual2 = Percentual.Criar(10m);

        (percentual1 < percentual2).Should().BeTrue();
        (percentual2 < percentual1).Should().BeFalse();
    }

    [Fact]
    public void CompareTo_DeveRetornarResultadoCorreto()
    {
        var percentual1 = Percentual.Criar(5m);
        var percentual2 = Percentual.Criar(10m);
        var percentual3 = Percentual.Criar(5m);

        percentual1.CompareTo(percentual2).Should().BeLessThan(0);
        percentual2.CompareTo(percentual1).Should().BeGreaterThan(0);
        percentual1.CompareTo(percentual3).Should().Be(0);
    }

    [Fact]
    public void ToString_DeveRetornarValorFormatadoComPorcentagem()
    {
        var percentual = Percentual.Criar(5.5m);

        percentual.ToString().Should().Be("5,50%");
    }

    [Fact]
    public void ImplicitOperator_DeveConverterParaDecimal()
    {
        var percentual = Percentual.Criar(5m);

        decimal valor = percentual;

        valor.Should().Be(5m);
    }

    [Fact]
    public void Equals_ComNull_DeveRetornarFalse()
    {
        var percentual = Percentual.Criar(5m);

        percentual.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Equals_ComMesmaReferencia_DeveRetornarTrue()
    {
        var percentual = Percentual.Criar(5m);

        percentual.Equals(percentual).Should().BeTrue();
    }

    [Fact]
    public void EqualsObject_ComPercentualIgual_DeveRetornarTrue()
    {
        var percentual1 = Percentual.Criar(5m);
        object percentual2 = Percentual.Criar(5m);

        percentual1.Equals(percentual2).Should().BeTrue();
    }

    [Fact]
    public void EqualsObject_ComOutroTipo_DeveRetornarFalse()
    {
        var percentual = Percentual.Criar(5m);
        object outro = "teste";

        percentual.Equals(outro).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ComMesmoValor_DeveRetornarMesmoHash()
    {
        var percentual1 = Percentual.Criar(5m);
        var percentual2 = Percentual.Criar(5m);

        percentual1.GetHashCode().Should().Be(percentual2.GetHashCode());
    }

    [Fact]
    public void CompareTo_ComNull_DeveRetornar1()
    {
        var percentual = Percentual.Criar(5m);

        percentual.CompareTo(null).Should().Be(1);
    }

    [Fact]
    public void OperadorMaiorOuIgual_DeveRetornarResultadoCorreto()
    {
        var percentual1 = Percentual.Criar(10m);
        var percentual2 = Percentual.Criar(5m);
        var percentual3 = Percentual.Criar(10m);

        (percentual1 >= percentual2).Should().BeTrue();
        (percentual1 >= percentual3).Should().BeTrue();
        (percentual2 >= percentual1).Should().BeFalse();
    }

    [Fact]
    public void OperadorMenorOuIgual_DeveRetornarResultadoCorreto()
    {
        var percentual1 = Percentual.Criar(5m);
        var percentual2 = Percentual.Criar(10m);
        var percentual3 = Percentual.Criar(5m);

        (percentual1 <= percentual2).Should().BeTrue();
        (percentual1 <= percentual3).Should().BeTrue();
        (percentual2 <= percentual1).Should().BeFalse();
    }
}
