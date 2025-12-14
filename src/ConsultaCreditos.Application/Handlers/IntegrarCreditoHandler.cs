using ConsultaCreditos.Application.Commands;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Interfaces;
using ConsultaCreditos.Application.Validators;
using ConsultaCreditos.Domain.Exceptions;
using FluentValidation;

namespace ConsultaCreditos.Application.Handlers;

public class IntegrarCreditoHandler
{
    private readonly IServiceBusPublisher _serviceBusPublisher;
    private readonly IntegrarCreditoRequestValidator _validator;

    public IntegrarCreditoHandler(IServiceBusPublisher serviceBusPublisher)
    {
        _serviceBusPublisher = serviceBusPublisher;
        _validator = new IntegrarCreditoRequestValidator();
    }

    public async Task<IntegrarCreditoResponse> Handle(IntegrarCreditoCommand command, CancellationToken cancellationToken = default)
    {
        foreach (var credito in command.Creditos)
        {
            var validationResult = await _validator.ValidateAsync(credito, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new DomainException($"Erro de validação: {errors}");
            }

            await _serviceBusPublisher.PublishAsync(credito, cancellationToken);
        }

        return IntegrarCreditoResponse.Sucesso();
    }
}
