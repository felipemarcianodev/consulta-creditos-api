using ConsultaCreditos.Domain.Entities;

namespace ConsultaCreditos.Domain.Interfaces;

public interface ICreditoRepository
{
    Task AdicionarAsync(Credito credito, CancellationToken cancellationToken = default);
    Task<List<Credito>> ObterPorNumeroNfseAsync(string numeroNfse, CancellationToken cancellationToken = default);
    Task<Credito?> ObterPorNumeroCreditoAsync(string numeroCredito, CancellationToken cancellationToken = default);
    Task<bool> ExisteAsync(string numeroCredito, CancellationToken cancellationToken = default);
}
