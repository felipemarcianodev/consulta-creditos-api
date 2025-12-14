using ConsultaCreditos.Application.DTOs;

namespace ConsultaCreditos.Application.Interfaces;

public interface IServiceBusPublisher
{
    Task PublishAsync<T>(T data, CancellationToken cancellationToken = default) where T: class;
}
