using ConsultaCreditos.Domain.Exceptions;
using FluentAssertions;

namespace ConsultaCreditos.UnitTests.Domain.Exceptions;

public class CreditoDuplicadoExceptionTests
{
    [Fact]
    public void DeveCriarExcecaoComNumeroCredito()
    {
        var numeroCredito = "123456";

        var exception = new CreditoDuplicadoException(numeroCredito);

        exception.NumeroCredito.Should().Be(numeroCredito);
        exception.Message.Should().Contain(numeroCredito);
        exception.Message.Should().Contain("já existe");
    }

    [Fact]
    public void DeveSerDomainException()
    {
        var numeroCredito = "123456";

        var exception = new CreditoDuplicadoException(numeroCredito);

        exception.Should().BeAssignableTo<DomainException>();
    }
}
