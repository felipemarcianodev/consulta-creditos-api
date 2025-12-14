using ConsultaCreditos.Application.DTOs;

namespace ConsultaCreditos.Application.Interfaces;

public interface IServiceBusPublisher
{
    Task PublicarAsync(IntegrarCreditoRequest credito, CancellationToken cancellationToken = default);
}
