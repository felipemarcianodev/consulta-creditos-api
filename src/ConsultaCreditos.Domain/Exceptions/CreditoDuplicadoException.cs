namespace ConsultaCreditos.Domain.Exceptions;

public class CreditoDuplicadoException : DomainException
{
    public string NumeroCredito { get; }

    public CreditoDuplicadoException(string numeroCredito)
        : base($"Crédito com número {numeroCredito} já existe")
    {
        NumeroCredito = numeroCredito;
    }
}
