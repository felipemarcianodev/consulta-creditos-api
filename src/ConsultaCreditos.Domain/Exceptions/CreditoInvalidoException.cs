namespace ConsultaCreditos.Domain.Exceptions;

public class CreditoInvalidoException : DomainException
{
    public CreditoInvalidoException(string message) : base(message)
    {
    }

    public CreditoInvalidoException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
