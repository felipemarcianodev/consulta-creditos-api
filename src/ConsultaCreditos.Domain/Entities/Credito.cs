using ConsultaCreditos.Domain.Enums;
using ConsultaCreditos.Domain.Exceptions;

namespace ConsultaCreditos.Domain.Entities;

public class Credito
{
    public long Id { get; private set; }
    public string NumeroCredito { get; private set; }
    public string NumeroNfse { get; private set; }
    public DateTime DataConstituicao { get; private set; }
    public decimal ValorIssqn { get; private set; }
    public TipoCredito TipoCredito { get; private set; }
    public bool SimplesNacional { get; private set; }
    public decimal Aliquota { get; private set; }
    public decimal ValorFaturado { get; private set; }
    public decimal ValorDeducao { get; private set; }
    public decimal BaseCalculo { get; private set; }

    private Credito()
    {
        NumeroCredito = string.Empty;
        NumeroNfse = string.Empty;
    }

    public static Credito Criar(
        string numeroCredito,
        string numeroNfse,
        DateTime dataConstituicao,
        decimal valorIssqn,
        TipoCredito tipoCredito,
        bool simplesNacional,
        decimal aliquota,
        decimal valorFaturado,
        decimal valorDeducao,
        decimal baseCalculo)
    {
        ValidarDados(numeroCredito, numeroNfse, valorIssqn, aliquota, valorFaturado, valorDeducao, baseCalculo);

        var credito = new Credito
        {
            NumeroCredito = numeroCredito,
            NumeroNfse = numeroNfse,
            DataConstituicao = dataConstituicao,
            TipoCredito = tipoCredito,
            SimplesNacional = simplesNacional,
            Aliquota = aliquota,
            ValorFaturado = valorFaturado,
            ValorDeducao = valorDeducao,
            BaseCalculo = baseCalculo,
            ValorIssqn = valorIssqn
        };

        credito.ValidarInvariantes();
        return credito;
    }

    private static void ValidarDados(
        string numeroCredito,
        string numeroNfse,
        decimal valorIssqn,
        decimal aliquota,
        decimal valorFaturado,
        decimal valorDeducao,
        decimal baseCalculo)
    {
        if (string.IsNullOrWhiteSpace(numeroCredito))
            throw new CreditoInvalidoException("Número do crédito é obrigatório");

        if (string.IsNullOrWhiteSpace(numeroNfse))
            throw new CreditoInvalidoException("Número da NFS-e é obrigatório");

        if (valorIssqn <= 0)
            throw new CreditoInvalidoException("Valor do ISSQN deve ser maior que zero");

        if (aliquota < 0 || aliquota > 100)
            throw new CreditoInvalidoException("Alíquota deve estar entre 0 e 100");

        if (valorFaturado <= 0)
            throw new CreditoInvalidoException("Valor faturado deve ser maior que zero");

        if (valorDeducao < 0)
            throw new CreditoInvalidoException("Valor de dedução não pode ser negativo");

        if (baseCalculo <= 0)
            throw new CreditoInvalidoException("Base de cálculo deve ser maior que zero");
    }

    private void ValidarInvariantes()
    {
        if (DataConstituicao > DateTime.Now)
            throw new CreditoInvalidoException("Data de constituição não pode ser futura");

        var baseCalculoEsperada = ValorFaturado - ValorDeducao;
        if (Math.Abs(BaseCalculo - baseCalculoEsperada) > 0.01m)
            throw new CreditoInvalidoException(
                $"Base de cálculo inválida. Esperado: {baseCalculoEsperada:F2}, Recebido: {BaseCalculo:F2}");

        var valorIssqnEsperado = BaseCalculo * (Aliquota / 100m);
        if (Math.Abs(ValorIssqn - valorIssqnEsperado) > 0.01m)
            throw new CreditoInvalidoException(
                $"Valor do ISSQN inválido. Esperado: {valorIssqnEsperado:F2}, Recebido: {ValorIssqn:F2}");
    }
}
