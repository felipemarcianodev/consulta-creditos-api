using ConsultaCreditos.Application.DTOs;

namespace ConsultaCreditos.Application.Interfaces;

public interface IServiceBusPublisher
{
    Task PublicarCreditoAsync(IntegrarCreditoRequest credito, CancellationToken cancellationToken = default);
}
