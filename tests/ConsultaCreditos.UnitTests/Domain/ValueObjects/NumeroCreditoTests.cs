using ConsultaCreditos.Domain.ValueObjects;
using FluentAssertions;

namespace ConsultaCreditos.UnitTests.Domain.ValueObjects;

public class NumeroCreditoTests
{
    [Fact]
    public void Criar_ComValorValido_DeveCriarNumeroCredito()
    {
        var valor = "123456";

        var numeroCredito = NumeroCredito.Criar(valor);

        numeroCredito.Valor.Should().Be(valor);
    }

    [Fact]
    public void Criar_ComValorComEspacos_DeveRemoverEspacos()
    {
        var valor = "  123456  ";

        var numeroCredito = NumeroCredito.Criar(valor);

        numeroCredito.Valor.Should().Be("123456");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_ComValorVazio_DeveLancarExcecao(string? valor)
    {
        var act = () => NumeroCredito.Criar(valor);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*não pode ser vazio*");
    }

    [Fact]
    public void Criar_ComValorMuitoLongo_DeveLancarExcecao()
    {
        var valor = new string('1', 51);

        var act = () => NumeroCredito.Criar(valor);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*não pode ter mais de 50 caracteres*");
    }

    [Fact]
    public void Equals_ComMesmoValor_DeveRetornarTrue()
    {
        var numero1 = NumeroCredito.Criar("123456");
        var numero2 = NumeroCredito.Criar("123456");

        numero1.Equals(numero2).Should().BeTrue();
        (numero1 == numero2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ComValoresDiferentes_DeveRetornarFalse()
    {
        var numero1 = NumeroCredito.Criar("123456");
        var numero2 = NumeroCredito.Criar("789012");

        numero1.Equals(numero2).Should().BeFalse();
        (numero1 != numero2).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_ComMesmoValor_DeveRetornarMesmoHash()
    {
        var numero1 = NumeroCredito.Criar("123456");
        var numero2 = NumeroCredito.Criar("123456");

        numero1.GetHashCode().Should().Be(numero2.GetHashCode());
    }

    [Fact]
    public void ToString_DeveRetornarValor()
    {
        var valor = "123456";
        var numeroCredito = NumeroCredito.Criar(valor);

        numeroCredito.ToString().Should().Be(valor);
    }

    [Fact]
    public void ImplicitOperator_DeveConverterParaString()
    {
        var numeroCredito = NumeroCredito.Criar("123456");

        string valor = numeroCredito;

        valor.Should().Be("123456");
    }
}
