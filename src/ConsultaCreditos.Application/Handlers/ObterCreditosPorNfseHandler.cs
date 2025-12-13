using AutoMapper;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Queries;
using ConsultaCreditos.Domain.Interfaces;

namespace ConsultaCreditos.Application.Handlers;

public class ObterCreditosPorNfseHandler
{
    private readonly ICreditoRepository _creditoRepository;
    private readonly IMapper _mapper;

    public ObterCreditosPorNfseHandler(ICreditoRepository creditoRepository, IMapper mapper)
    {
        _creditoRepository = creditoRepository;
        _mapper = mapper;
    }

    public async Task<List<CreditoDto>> Handle(ObterCreditosPorNfseQuery query, CancellationToken cancellationToken = default)
    {
        var creditos = await _creditoRepository.ObterPorNumeroNfseAsync(query.NumeroNfse, cancellationToken);

        return _mapper.Map<List<CreditoDto>>(creditos);
    }
}
