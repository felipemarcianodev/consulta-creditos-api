using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Exceptions;
using ConsultaCreditos.Domain.ValueObjects;

namespace ConsultaCreditos.Domain.Services;

public class CreditoValidationService
{
    public void ValidarCalculos(Credito credito)
    {
        ValidarBaseCalculo(credito.ValorFaturado, credito.ValorDeducao, credito.BaseCalculo);
        ValidarValorIssqn(credito.BaseCalculo, credito.Aliquota, credito.ValorIssqn);
    }

    public void ValidarBaseCalculo(Dinheiro valorFaturado, Dinheiro valorDeducao, Dinheiro baseCalculo)
    {
        var baseCalculoEsperada = valorFaturado - valorDeducao;

        if (Math.Abs(baseCalculo.Valor - baseCalculoEsperada.Valor) > 0.01m)
            throw new CreditoInvalidoException(
                $"Base de cálculo inválida. Esperado: {baseCalculoEsperada}, Recebido: {baseCalculo}");
    }

    public void ValidarValorIssqn(Dinheiro baseCalculo, Percentual aliquota, Dinheiro valorIssqn)
    {
        var valorIssqnEsperado = aliquota.AplicarSobre(baseCalculo);

        if (Math.Abs(valorIssqn.Valor - valorIssqnEsperado.Valor) > 0.01m)
            throw new CreditoInvalidoException(
                $"Valor do ISSQN inválido. Esperado: {valorIssqnEsperado}, Recebido: {valorIssqn}");
    }

    public void ValidarConsistencia(Credito credito)
    {
        if (credito.DataConstituicao > DateTime.Now)
            throw new CreditoInvalidoException("Data de constituição não pode ser futura");

        if (credito.ValorIssqn <= Dinheiro.Zero)
            throw new CreditoInvalidoException("Valor do ISSQN deve ser maior que zero");

        if (credito.ValorFaturado < credito.ValorDeducao)
            throw new CreditoInvalidoException("Valor dedução não pode ser maior que valor faturado");

        ValidarCalculos(credito);
    }
}
