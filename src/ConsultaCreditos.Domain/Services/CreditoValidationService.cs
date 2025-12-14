using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Exceptions;

namespace ConsultaCreditos.Domain.Services;

public class CreditoValidationService
{
    public void ValidarCalculos(Credito credito)
    {
        ValidarBaseCalculo(credito.ValorFaturado, credito.ValorDeducao, credito.BaseCalculo);
        ValidarValorIssqn(credito.BaseCalculo, credito.Aliquota, credito.ValorIssqn);
    }

    public void ValidarBaseCalculo(decimal valorFaturado, decimal valorDeducao, decimal baseCalculo)
    {
        var baseCalculoEsperada = valorFaturado - valorDeducao;

        if (Math.Abs(baseCalculo - baseCalculoEsperada) > 0.01m)
            throw new CreditoInvalidoException(
                $"Base de cálculo inválida. Esperado: {baseCalculoEsperada:F2}, Recebido: {baseCalculo:F2}");
    }

    public void ValidarValorIssqn(decimal baseCalculo, decimal aliquota, decimal valorIssqn)
    {
        var valorIssqnEsperado = baseCalculo * (aliquota / 100m);

        if (Math.Abs(valorIssqn - valorIssqnEsperado) > 0.01m)
            throw new CreditoInvalidoException(
                $"Valor do ISSQN inválido. Esperado: {valorIssqnEsperado:F2}, Recebido: {valorIssqn:F2}");
    }

    public void ValidarConsistencia(Credito credito)
    {
        if (credito.DataConstituicao > DateTime.Now)
            throw new CreditoInvalidoException("Data de constituição não pode ser futura");

        if (credito.ValorIssqn <= 0)
            throw new CreditoInvalidoException("Valor do ISSQN deve ser maior que zero");

        if (credito.ValorFaturado < credito.ValorDeducao)
            throw new CreditoInvalidoException("Valor dedução não pode ser maior que valor faturado");

        ValidarCalculos(credito);
    }
}
