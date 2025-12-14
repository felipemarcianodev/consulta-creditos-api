using AutoMapper;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Interfaces;
using ConsultaCreditos.Application.Queries;
using ConsultaCreditos.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConsultaCreditos.Application.Handlers;

public class ObterCreditosPorNfseHandler
{
    private readonly ICreditoRepository _creditoRepository;
    private readonly IMapper _mapper;
    private readonly IServiceBusPublisher _serviceBusPublisher;
    private readonly ILogger<ObterCreditosPorNfseHandler> _logger;

    public ObterCreditosPorNfseHandler(
        ICreditoRepository creditoRepository, 
        IMapper mapper, 
        IServiceBusPublisher serviceBusPublisher, 
        ILogger<ObterCreditosPorNfseHandler> logger)
    {
        _creditoRepository = creditoRepository;
        _mapper = mapper;
        _serviceBusPublisher = serviceBusPublisher;
        _logger = logger;
    }

    public async Task<List<CreditoDto>> Handle(ObterCreditosPorNfseQuery query, CancellationToken cancellationToken = default)
    {
        var creditos = await _creditoRepository.ObterPorNumeroNfseAsync(query.NumeroNfse, cancellationToken);

        var auditoriaEvent = new AuditoriaConsultaEvent
        {
            TipoConsulta = "ConsultaPorNfse",
            NumeroReferencia = query.NumeroNfse,
            Detalhes = $"Consulta realizada para NFSe: {query.NumeroNfse}"
        };

        await _serviceBusPublisher.PublishAsync(auditoriaEvent, cancellationToken);
        _logger.LogInformation("Auditoria de consulta publicada para NFSe: {NumeroNfse}", query.NumeroNfse);

        return _mapper.Map<List<CreditoDto>>(creditos);
    }
}
