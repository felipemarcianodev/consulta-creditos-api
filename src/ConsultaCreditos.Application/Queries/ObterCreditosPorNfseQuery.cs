namespace ConsultaCreditos.Application.Queries;

public class ObterCreditosPorNfseQuery
{
    public string NumeroNfse { get; set; }

    public ObterCreditosPorNfseQuery(string numeroNfse)
    {
        NumeroNfse = numeroNfse;
    }
}
