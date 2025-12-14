using ConsultaCreditos.Domain.Exceptions;
using FluentAssertions;

namespace ConsultaCreditos.UnitTests.Domain.Exceptions;

public class CreditoInvalidoExceptionTests
{
    [Fact]
    public void DeveCriarExcecaoComMensagem()
    {
        var mensagem = "Crédito inválido";

        var exception = new CreditoInvalidoException(mensagem);

        exception.Message.Should().Be(mensagem);
    }

    [Fact]
    public void DeveCriarExcecaoComMensagemEInnerException()
    {
        var mensagem = "Crédito inválido";
        var innerException = new InvalidOperationException("Erro interno");

        var exception = new CreditoInvalidoException(mensagem, innerException);

        exception.Message.Should().Be(mensagem);
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void DeveSerDomainException()
    {
        var exception = new CreditoInvalidoException("Teste");

        exception.Should().BeAssignableTo<DomainException>();
    }
}
