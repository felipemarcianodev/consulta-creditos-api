using ConsultaCreditos.Application.Commands;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Handlers;
using ConsultaCreditos.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaCreditos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditosController : ControllerBase
{
    private readonly IntegrarCreditoHandler _integrarHandler;
    private readonly ObterCreditosPorNfseHandler _obterPorNfseHandler;
    private readonly ObterCreditoPorNumeroHandler _obterPorNumeroHandler;
    private readonly ILogger<CreditosController> _logger;

    public CreditosController(
        IntegrarCreditoHandler integrarHandler,
        ObterCreditosPorNfseHandler obterPorNfseHandler,
        ObterCreditoPorNumeroHandler obterPorNumeroHandler,
        ILogger<CreditosController> logger)
    {
        _integrarHandler = integrarHandler;
        _obterPorNfseHandler = obterPorNfseHandler;
        _obterPorNumeroHandler = obterPorNumeroHandler;
        _logger = logger;
    }

    [HttpPost("integrar-credito-constituido")]
    [ProducesResponseType(typeof(IntegrarCreditoResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> IntegrarCreditoConstituido(
        [FromBody] List<IntegrarCreditoRequest> creditos,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Recebida solicitação para integrar {Count} créditos", creditos.Count);

        var command = new IntegrarCreditoCommand(creditos);
        var result = await _integrarHandler.Handle(command, cancellationToken);

        _logger.LogInformation("Créditos publicados no Service Bus com sucesso");

        return AcceptedAtAction(null, result);
    }

    [HttpGet("{numeroNfse}")]
    [ProducesResponseType(typeof(List<CreditoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterCreditosPorNfse(
        string numeroNfse,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consultando créditos por NFS-e: {NumeroNfse}", numeroNfse);

        var query = new ObterCreditosPorNfseQuery(numeroNfse);
        var result = await _obterPorNfseHandler.Handle(query, cancellationToken);

        _logger.LogInformation("Encontrados {Count} créditos para NFS-e: {NumeroNfse}", result.Count, numeroNfse);

        return Ok(result);
    }

    [HttpGet("credito/{numeroCredito}")]
    [ProducesResponseType(typeof(CreditoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterCreditoPorNumero(
        string numeroCredito,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consultando crédito por número: {NumeroCredito}", numeroCredito);

        var query = new ObterCreditoPorNumeroQuery(numeroCredito);
        var result = await _obterPorNumeroHandler.Handle(query, cancellationToken);

        if (result == null)
        {
            _logger.LogWarning("Crédito não encontrado: {NumeroCredito}", numeroCredito);
            return NotFound();
        }

        _logger.LogInformation("Crédito encontrado: {NumeroCredito}", numeroCredito);

        return Ok(result);
    }
}
