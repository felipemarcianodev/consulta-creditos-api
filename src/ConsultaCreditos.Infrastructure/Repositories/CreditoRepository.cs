using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Interfaces;
using ConsultaCreditos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ConsultaCreditos.Infrastructure.Repositories;

[ExcludeFromCodeCoverage]
public class CreditoRepository : ICreditoRepository
{
    private readonly ApplicationDbContext _context;

    public CreditoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Credito credito, CancellationToken cancellationToken = default)
    {
        await _context.Creditos.AddAsync(credito, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Credito>> ObterPorNumeroNfseAsync(string numeroNfse, CancellationToken cancellationToken = default)
    {
        return await _context.Creditos
            .AsNoTracking()
            .Where(c => c.NumeroNfse.Valor == numeroNfse)
            .OrderByDescending(c => c.DataConstituicao)
            .ToListAsync(cancellationToken);
    }

    public async Task<Credito?> ObterPorNumeroCreditoAsync(string numeroCredito, CancellationToken cancellationToken = default)
    {
        return await _context.Creditos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.NumeroCredito.Valor == numeroCredito, cancellationToken);
    }

    public async Task<bool> ExisteAsync(string numeroCredito, CancellationToken cancellationToken = default)
    {
        return await _context.Creditos
            .AsNoTracking()
            .AnyAsync(c => c.NumeroCredito.Valor == numeroCredito, cancellationToken);
    }
}
