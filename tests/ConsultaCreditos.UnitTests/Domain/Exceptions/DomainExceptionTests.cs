using ConsultaCreditos.Domain.Exceptions;
using FluentAssertions;

namespace ConsultaCreditos.UnitTests.Domain.Exceptions;

public class DomainExceptionTests
{
    [Fact]
    public void DeveCriarExcecaoComMensagem()
    {
        var mensagem = "Erro de domínio";

        var exception = new DomainException(mensagem);

        exception.Message.Should().Be(mensagem);
    }

    [Fact]
    public void DeveCriarExcecaoComMensagemEInnerException()
    {
        var mensagem = "Erro de domínio";
        var innerException = new InvalidOperationException("Erro interno");

        var exception = new DomainException(mensagem, innerException);

        exception.Message.Should().Be(mensagem);
        exception.InnerException.Should().Be(innerException);
    }
}
