using ConsultaCreditos.Domain.ValueObjects;
using FluentAssertions;

namespace ConsultaCreditos.UnitTests.Domain.ValueObjects;

public class NumeroNfseTests
{
    [Fact]
    public void Criar_ComValorValido_DeveCriarNumeroNfse()
    {
        var valor = "7891011";

        var numeroNfse = NumeroNfse.Criar(valor);

        numeroNfse.Valor.Should().Be(valor);
    }

    [Fact]
    public void Criar_ComValorComEspacos_DeveRemoverEspacos()
    {
        var valor = "  7891011  ";

        var numeroNfse = NumeroNfse.Criar(valor);

        numeroNfse.Valor.Should().Be("7891011");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_ComValorVazio_DeveLancarExcecao(string valor)
    {
        var act = () => NumeroNfse.Criar(valor);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*não pode ser vazio*");
    }

    [Fact]
    public void Criar_ComValorMuitoLongo_DeveLancarExcecao()
    {
        var valor = new string('1', 51);

        var act = () => NumeroNfse.Criar(valor);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*não pode ter mais de 50 caracteres*");
    }

    [Fact]
    public void Equals_ComMesmoValor_DeveRetornarTrue()
    {
        var numero1 = NumeroNfse.Criar("7891011");
        var numero2 = NumeroNfse.Criar("7891011");

        numero1.Equals(numero2).Should().BeTrue();
        (numero1 == numero2).Should().BeTrue();
    }

    [Fact]
    public void Equals_ComValoresDiferentes_DeveRetornarFalse()
    {
        var numero1 = NumeroNfse.Criar("7891011");
        var numero2 = NumeroNfse.Criar("1122334");

        numero1.Equals(numero2).Should().BeFalse();
        (numero1 != numero2).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_ComMesmoValor_DeveRetornarMesmoHash()
    {
        var numero1 = NumeroNfse.Criar("7891011");
        var numero2 = NumeroNfse.Criar("7891011");

        numero1.GetHashCode().Should().Be(numero2.GetHashCode());
    }

    [Fact]
    public void ToString_DeveRetornarValor()
    {
        var valor = "7891011";
        var numeroNfse = NumeroNfse.Criar(valor);

        numeroNfse.ToString().Should().Be(valor);
    }

    [Fact]
    public void ImplicitOperator_DeveConverterParaString()
    {
        var numeroNfse = NumeroNfse.Criar("7891011");

        string valor = numeroNfse;

        valor.Should().Be("7891011");
    }
}
