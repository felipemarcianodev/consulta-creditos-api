using AutoMapper;
using ConsultaCreditos.Application.Commands;
using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ConsultaCreditos.Application.Handlers;

public class ProcessarCreditoHandler
{
    private readonly ICreditoRepository _creditoRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProcessarCreditoHandler> _logger;

    public ProcessarCreditoHandler(
        ICreditoRepository creditoRepository,
        IMapper mapper,
        ILogger<ProcessarCreditoHandler> logger)
    {
        _creditoRepository = creditoRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(ProcessarCreditoCommand command, CancellationToken cancellationToken = default)
    {
        var numeroCredito = command.Credito.NumeroCredito;

        var existe = await _creditoRepository.ExisteAsync(numeroCredito, cancellationToken);

        if (existe)
        {
            _logger.LogInformation("Crédito {NumeroCredito} já existe. Descartando mensagem (idempotência).", numeroCredito);
            return;
        }

        var credito = _mapper.Map<Credito>(command.Credito);

        await _creditoRepository.AdicionarAsync(credito, cancellationToken);

        _logger.LogInformation("Crédito {NumeroCredito} processado e persistido com sucesso.", numeroCredito);
    }
}
