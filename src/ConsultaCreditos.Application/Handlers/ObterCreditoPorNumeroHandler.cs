using AutoMapper;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Queries;
using ConsultaCreditos.Domain.Interfaces;

namespace ConsultaCreditos.Application.Handlers;

public class ObterCreditoPorNumeroHandler
{
    private readonly ICreditoRepository _creditoRepository;
    private readonly IMapper _mapper;

    public ObterCreditoPorNumeroHandler(ICreditoRepository creditoRepository, IMapper mapper)
    {
        _creditoRepository = creditoRepository;
        _mapper = mapper;
    }

    public async Task<CreditoDto?> Handle(ObterCreditoPorNumeroQuery query, CancellationToken cancellationToken = default)
    {
        var credito = await _creditoRepository.ObterPorNumeroCreditoAsync(query.NumeroCredito, cancellationToken);

        if (credito == null)
            return null;

        return _mapper.Map<CreditoDto>(credito);
    }
}
