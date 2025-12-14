using ConsultaCreditos.Domain.ValueObjects;
using FluentAssertions;

namespace ConsultaCreditos.UnitTests.Domain.ValueObjects;

public class DinheiroTests
{
    [Fact]
    public void Criar_ComValorValido_DeveCriarDinheiro()
    {
        var valor = 1500.75m;

        var dinheiro = Dinheiro.Criar(valor);

        dinheiro.Valor.Should().Be(valor);
    }

    [Fact]
    public void Criar_ComValorComMaisDeDuasCasasDecimais_DeveArredondar()
    {
        var valor = 1500.759m;

        var dinheiro = Dinheiro.Criar(valor);

        dinheiro.Valor.Should().Be(1500.76m);
    }

    [Fact]
    public void Criar_ComValorNegativo_DeveLancarExcecao()
    {
        var valor = -100m;

        var act = () => Dinheiro.Criar(valor);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*não pode ser negativo*");
    }

    [Fact]
    public void Zero_DeveRetornarDinheiroComValorZero()
    {
        var zero = Dinheiro.Zero;

        zero.Valor.Should().Be(0);
    }

    [Fact]
    public void Somar_DeveRetornarSomaCorreta()
    {
        var valor1 = Dinheiro.Criar(100m);
        var valor2 = Dinheiro.Criar(50m);

        var resultado = valor1.Somar(valor2);

        resultado.Valor.Should().Be(150m);
    }

    [Fact]
    public void Subtrair_DeveRetornarSubtracaoCorreta()
    {
        var valor1 = Dinheiro.Criar(100m);
        var valor2 = Dinheiro.Criar(50m);

        var resultado = valor1.Subtrair(valor2);

        resultado.Valor.Should().Be(50m);
    }

    [Fact]
    public void Multiplicar_DeveRetornarMultiplicacaoCorreta()
    {
        var valor = Dinheiro.Criar(100m);

        var resultado = valor.Multiplicar(1.5m);

        resultado.Valor.Should().Be(150m);
    }

    [Fact]
    public void Dividir_DeveRetornarDivisaoCorreta()
    {
        var valor = Dinheiro.Criar(100m);

        var resultado = valor.Dividir(2m);

        resultado.Valor.Should().Be(50m);
    }

    [Fact]
    public void Dividir_PorZero_DeveLancarExcecao()
    {
        var valor = Dinheiro.Criar(100m);

        var act = () => valor.Dividir(0);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*não pode ser zero*");
    }

    [Fact]
    public void OperadorSoma_DeveRetornarSomaCorreta()
    {
        var valor1 = Dinheiro.Criar(100m);
        var valor2 = Dinheiro.Criar(50m);

        var resultado = valor1 + valor2;

        resultado.Valor.Should().Be(150m);
    }

    [Fact]
    public void OperadorSubtracao_DeveRetornarSubtracaoCorreta()
    {
        var valor1 = Dinheiro.Criar(100m);
        var valor2 = Dinheiro.Criar(50m);

        var resultado = valor1 - valor2;

        resultado.Valor.Should().Be(50m);
    }

    [Fact]
    public void OperadorMultiplicacao_DeveRetornarMultiplicacaoCorreta()
    {
        var valor = Dinheiro.Criar(100m);

        var resultado = valor * 1.5m;

        resultado.Valor.Should().Be(150m);
    }

    [Fact]
    public void OperadorDivisao_DeveRetornarDivisaoCorreta()
    {
        var valor = Dinheiro.Criar(100m);

        var resultado = valor / 2m;

        resultado.Valor.Should().Be(50m);
    }

    [Fact]
    public void OperadorMaiorQue_DeveRetornarResultadoCorreto()
    {
        var valor1 = Dinheiro.Criar(100m);
        var valor2 = Dinheiro.Criar(50m);

        (valor1 > valor2).Should().BeTrue();
        (valor2 > valor1).Should().BeFalse();
    }

    [Fact]
    public void OperadorMenorQue_DeveRetornarResultadoCorreto()
    {
        var valor1 = Dinheiro.Criar(50m);
        var valor2 = Dinheiro.Criar(100m);

        (valor1 < valor2).Should().BeTrue();
        (valor2 < valor1).Should().BeFalse();
    }

    [Fact]
    public void Equals_ComMesmoValor_DeveRetornarTrue()
    {
        var valor1 = Dinheiro.Criar(100m);
        var valor2 = Dinheiro.Criar(100m);

        valor1.Equals(valor2).Should().BeTrue();
        (valor1 == valor2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ComValoresDiferentes_DeveRetornarFalse()
    {
        var valor1 = Dinheiro.Criar(100m);
        var valor2 = Dinheiro.Criar(50m);

        valor1.Equals(valor2).Should().BeFalse();
        (valor1 != valor2).Should().BeTrue();
    }

    [Fact]
    public void CompareTo_DeveRetornarResultadoCorreto()
    {
        var valor1 = Dinheiro.Criar(50m);
        var valor2 = Dinheiro.Criar(100m);
        var valor3 = Dinheiro.Criar(50m);

        valor1.CompareTo(valor2).Should().BeLessThan(0);
        valor2.CompareTo(valor1).Should().BeGreaterThan(0);
        valor1.CompareTo(valor3).Should().Be(0);
    }

    [Fact]
    public void ToString_DeveRetornarValorFormatado()
    {
        var dinheiro = Dinheiro.Criar(1500.75m);

        dinheiro.ToString().Should().Be("1.500,75");
    }

    [Fact]
    public void ImplicitOperator_DeveConverterParaDecimal()
    {
        var dinheiro = Dinheiro.Criar(1500.75m);

        decimal valor = dinheiro;

        valor.Should().Be(1500.75m);
    }

    [Fact]
    public void Equals_ComNull_DeveRetornarFalse()
    {
        var dinheiro = Dinheiro.Criar(100m);

        dinheiro.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Equals_ComMesmaReferencia_DeveRetornarTrue()
    {
        var dinheiro = Dinheiro.Criar(100m);

        dinheiro.Equals(dinheiro).Should().BeTrue();
    }

    [Fact]
    public void EqualsObject_ComDinheiroIgual_DeveRetornarTrue()
    {
        var dinheiro1 = Dinheiro.Criar(100m);
        object dinheiro2 = Dinheiro.Criar(100m);

        dinheiro1.Equals(dinheiro2).Should().BeTrue();
    }

    [Fact]
    public void EqualsObject_ComOutroTipo_DeveRetornarFalse()
    {
        var dinheiro = Dinheiro.Criar(100m);
        object outro = "teste";

        dinheiro.Equals(outro).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ComMesmoValor_DeveRetornarMesmoHash()
    {
        var dinheiro1 = Dinheiro.Criar(100m);
        var dinheiro2 = Dinheiro.Criar(100m);

        dinheiro1.GetHashCode().Should().Be(dinheiro2.GetHashCode());
    }

    [Fact]
    public void CompareTo_ComNull_DeveRetornar1()
    {
        var dinheiro = Dinheiro.Criar(100m);

        dinheiro.CompareTo(null).Should().Be(1);
    }

    [Fact]
    public void OperadorMaiorOuIgual_DeveRetornarResultadoCorreto()
    {
        var dinheiro1 = Dinheiro.Criar(100m);
        var dinheiro2 = Dinheiro.Criar(50m);
        var dinheiro3 = Dinheiro.Criar(100m);

        (dinheiro1 >= dinheiro2).Should().BeTrue();
        (dinheiro1 >= dinheiro3).Should().BeTrue();
        (dinheiro2 >= dinheiro1).Should().BeFalse();
    }

    [Fact]
    public void OperadorMenorOuIgual_DeveRetornarResultadoCorreto()
    {
        var dinheiro1 = Dinheiro.Criar(50m);
        var dinheiro2 = Dinheiro.Criar(100m);
        var dinheiro3 = Dinheiro.Criar(50m);

        (dinheiro1 <= dinheiro2).Should().BeTrue();
        (dinheiro1 <= dinheiro3).Should().BeTrue();
        (dinheiro2 <= dinheiro1).Should().BeFalse();
    }
}
