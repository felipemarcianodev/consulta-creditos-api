using ConsultaCreditos.Application.DTOs;
using FluentValidation;

namespace ConsultaCreditos.Application.Validators;

public class IntegrarCreditoRequestValidator : AbstractValidator<IntegrarCreditoRequest>
{
    public IntegrarCreditoRequestValidator()
    {
        RuleFor(x => x.NumeroCredito)
            .NotEmpty()
            .WithMessage("Número do crédito é obrigatório")
            .MaximumLength(50)
            .WithMessage("Número do crédito não pode ter mais de 50 caracteres");

        RuleFor(x => x.NumeroNfse)
            .NotEmpty()
            .WithMessage("Número da NFS-e é obrigatório")
            .MaximumLength(50)
            .WithMessage("Número da NFS-e não pode ter mais de 50 caracteres");

        RuleFor(x => x.DataConstituicao)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Data de constituição não pode ser futura");

        RuleFor(x => x.ValorIssqn)
            .GreaterThan(0)
            .WithMessage("Valor do ISSQN deve ser maior que zero");

        RuleFor(x => x.TipoCredito)
            .NotEmpty()
            .WithMessage("Tipo de crédito é obrigatório")
            .Must(tipo => tipo == "ISSQN" || tipo == "Outros")
            .WithMessage("Tipo de crédito deve ser 'ISSQN' ou 'Outros'");

        RuleFor(x => x.SimplesNacional)
            .NotEmpty()
            .WithMessage("Simples Nacional é obrigatório")
            .Must(sn => sn == "Sim" || sn == "Não")
            .WithMessage("Simples Nacional deve ser 'Sim' ou 'Não'");

        RuleFor(x => x.Aliquota)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Alíquota deve ser maior ou igual a zero")
            .LessThanOrEqualTo(100)
            .WithMessage("Alíquota deve ser menor ou igual a 100");

        RuleFor(x => x.ValorFaturado)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Valor faturado deve ser maior ou igual a zero");

        RuleFor(x => x.ValorDeducao)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Valor dedução deve ser maior ou igual a zero");

        RuleFor(x => x.BaseCalculo)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Base de cálculo deve ser maior ou igual a zero");

        RuleFor(x => x)
            .Must(ValidarBaseCalculo)
            .WithMessage("Base de cálculo inválida. Deve ser igual a Valor Faturado - Valor Dedução")
            .Must(ValidarValorIssqn)
            .WithMessage("Valor do ISSQN inválido. Deve ser igual a Base de Cálculo × Alíquota");
    }

    private bool ValidarBaseCalculo(IntegrarCreditoRequest request)
    {
        var baseCalculoEsperada = request.ValorFaturado - request.ValorDeducao;
        return Math.Abs(request.BaseCalculo - baseCalculoEsperada) <= 0.01m;
    }

    private bool ValidarValorIssqn(IntegrarCreditoRequest request)
    {
        var valorIssqnEsperado = request.BaseCalculo * (request.Aliquota / 100);
        return Math.Abs(request.ValorIssqn - valorIssqnEsperado) <= 0.01m;
    }
}
