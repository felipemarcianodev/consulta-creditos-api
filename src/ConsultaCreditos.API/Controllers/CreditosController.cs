using ConsultaCreditos.Application.Commands;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Handlers;
using ConsultaCreditos.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaCreditos.API.Controllers;

/// <summary>
/// Controller para integração e consulta de créditos constituídos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
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

    /// <summary>
    /// Integra uma lista de créditos constituídos no sistema
    /// </summary>
    /// <param name="creditos">Lista de créditos a serem integrados</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Confirmação de sucesso</returns>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     POST /api/creditos/integrar-credito-constituido
    ///     [
    ///       {
    ///         "numeroCredito": "123456",
    ///         "numeroNfse": "7891011",
    ///         "dataConstituicao": "2024-02-25",
    ///         "valorIssqn": 1500.75,
    ///         "tipoCredito": "ISSQN",
    ///         "simplesNacional": "Sim",
    ///         "aliquota": 5.0,
    ///         "valorFaturado": 30000.0,
    ///         "valorDeducao": 5000.0,
    ///         "baseCalculo": 25000.0
    ///       }
    ///     ]
    ///
    /// Cada crédito será publicado como mensagem individual no Azure Service Bus
    /// para processamento assíncrono pelo background service.
    /// </remarks>
    /// <response code="202">Créditos publicados com sucesso no Service Bus para processamento assíncrono</response>
    /// <response code="400">Dados inválidos na requisição. Verifique o formato e valores dos campos</response>
    /// <response code="500">Erro interno ao processar a requisição</response>
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

    /// <summary>
    /// Obtém todos os créditos associados a um número de NFS-e
    /// </summary>
    /// <param name="numeroNfse">Número da Nota Fiscal de Serviços Eletrônica</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de créditos encontrados para a NFS-e informada</returns>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     GET /api/creditos/7891011
    ///
    /// Retorna todos os créditos que foram constituídos para a NFS-e informada.
    /// Caso não existam créditos, retorna uma lista vazia.
    /// </remarks>
    /// <response code="200">Lista de créditos encontrados (pode ser vazia)</response>
    /// <response code="400">Número de NFS-e inválido</response>
    /// <response code="500">Erro interno ao processar a requisição</response>
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

    /// <summary>
    /// Obtém um crédito específico pelo número do crédito constituído
    /// </summary>
    /// <param name="numeroCredito">Número identificador do crédito constituído</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do crédito encontrado</returns>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     GET /api/creditos/credito/123456
    ///
    /// Retorna os detalhes completos de um crédito específico.
    /// Caso o crédito não exista, retorna HTTP 404 Not Found.
    /// </remarks>
    /// <response code="200">Crédito encontrado com sucesso</response>
    /// <response code="404">Crédito não encontrado</response>
    /// <response code="400">Número de crédito inválido</response>
    /// <response code="500">Erro interno ao processar a requisição</response>
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
