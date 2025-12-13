namespace ConsultaCreditos.Application.Queries;

public class ObterCreditoPorNumeroQuery
{
    public string NumeroCredito { get; set; }

    public ObterCreditoPorNumeroQuery(string numeroCredito)
    {
        NumeroCredito = numeroCredito;
    }
}
