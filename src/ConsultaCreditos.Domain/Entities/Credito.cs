using ConsultaCreditos.Domain.Enums;
using ConsultaCreditos.Domain.Exceptions;
using ConsultaCreditos.Domain.ValueObjects;

namespace ConsultaCreditos.Domain.Entities;

public class Credito
{
    public long Id { get; private set; }
    public NumeroCredito NumeroCredito { get; private set; }
    public NumeroNfse NumeroNfse { get; private set; }
    public DateTime DataConstituicao { get; private set; }
    public Dinheiro ValorIssqn { get; private set; }
    public TipoCredito TipoCredito { get; private set; }
    public bool SimplesNacional { get; private set; }
    public Percentual Aliquota { get; private set; }
    public Dinheiro ValorFaturado { get; private set; }
    public Dinheiro ValorDeducao { get; private set; }
    public Dinheiro BaseCalculo { get; private set; }

    private Credito()
    {
        NumeroCredito = null!;
        NumeroNfse = null!;
        ValorIssqn = null!;
        Aliquota = null!;
        ValorFaturado = null!;
        ValorDeducao = null!;
        BaseCalculo = null!;
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
        var credito = new Credito
        {
            NumeroCredito = NumeroCredito.Criar(numeroCredito),
            NumeroNfse = NumeroNfse.Criar(numeroNfse),
            DataConstituicao = dataConstituicao,
            TipoCredito = tipoCredito,
            SimplesNacional = simplesNacional,
            Aliquota = Percentual.Criar(aliquota),
            ValorFaturado = Dinheiro.Criar(valorFaturado),
            ValorDeducao = Dinheiro.Criar(valorDeducao),
            BaseCalculo = Dinheiro.Criar(baseCalculo),
            ValorIssqn = Dinheiro.Criar(valorIssqn)
        };

        credito.ValidarInvariantes();
        return credito;
    }

    private void ValidarInvariantes()
    {
        if (DataConstituicao > DateTime.Now)
            throw new CreditoInvalidoException("Data de constituição não pode ser futura");

        if (ValorIssqn <= Dinheiro.Zero)
            throw new CreditoInvalidoException("Valor do ISSQN deve ser maior que zero");

        var baseCalculoEsperada = ValorFaturado - ValorDeducao;
        if (Math.Abs(BaseCalculo.Valor - baseCalculoEsperada.Valor) > 0.01m)
            throw new CreditoInvalidoException(
                $"Base de cálculo inválida. Esperado: {baseCalculoEsperada}, Recebido: {BaseCalculo}");

        var valorIssqnEsperado = Aliquota.AplicarSobre(BaseCalculo);
        if (Math.Abs(ValorIssqn.Valor - valorIssqnEsperado.Valor) > 0.01m)
            throw new CreditoInvalidoException(
                $"Valor do ISSQN inválido. Esperado: {valorIssqnEsperado}, Recebido: {ValorIssqn}");
    }
}
