using AutoMapper;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Interfaces;
using ConsultaCreditos.Application.Queries;
using ConsultaCreditos.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConsultaCreditos.Application.Handlers;

public class ObterCreditoPorNumeroHandler
{
    private readonly ICreditoRepository _creditoRepository;
    private readonly IMapper _mapper;
    private readonly IServiceBusPublisher _serviceBusPublisher;
    private readonly ILogger<ObterCreditoPorNumeroHandler> _logger;

    public ObterCreditoPorNumeroHandler(
        ICreditoRepository creditoRepository, 
        IMapper mapper, 
        IServiceBusPublisher serviceBusPublisher, 
        ILogger<ObterCreditoPorNumeroHandler> logger)
    {
        _creditoRepository = creditoRepository;
        _mapper = mapper;
        _serviceBusPublisher = serviceBusPublisher;
        _logger = logger;
    }

    public async Task<CreditoDto?> Handle(ObterCreditoPorNumeroQuery query, CancellationToken cancellationToken = default)
    {
        var credito = await _creditoRepository.ObterPorNumeroCreditoAsync(query.NumeroCredito, cancellationToken);

        var auditoriaEvent = new AuditoriaConsultaEvent
        {
            TipoConsulta = "ConsultaPorNumeroCredito",
            NumeroReferencia = query.NumeroCredito,
            Detalhes = $"Consulta realizada para Crédito: {query.NumeroCredito}"
        };

        await _serviceBusPublisher.PublishAsync(auditoriaEvent, cancellationToken);
        _logger.LogInformation("Auditoria de consulta publicada para Crédito: {NumeroCredito}", query.NumeroCredito);

        if (credito == null)
            return null;

        return _mapper.Map<CreditoDto>(credito);
    }
}
