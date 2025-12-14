namespace ConsultaCreditos.Application.DTOs;

/// <summary>
/// Dados de um crédito a ser integrado no sistema
/// </summary>
public class IntegrarCreditoRequest
{
    /// <summary>
    /// Número identificador do crédito constituído (obrigatório, único)
    /// </summary>
    /// <example>123456</example>
    public string NumeroCredito { get; set; } = string.Empty;

    /// <summary>
    /// Número da Nota Fiscal de Serviços Eletrônica (obrigatório)
    /// </summary>
    /// <example>7891011</example>
    public string NumeroNfse { get; set; } = string.Empty;

    /// <summary>
    /// Data em que o crédito foi constituído (obrigatório, formato: YYYY-MM-DD)
    /// </summary>
    /// <example>2024-02-25</example>
    public DateTime DataConstituicao { get; set; }

    /// <summary>
    /// Valor do Imposto Sobre Serviços de Qualquer Natureza (obrigatório, maior que zero)
    /// </summary>
    /// <example>1500.75</example>
    public decimal ValorIssqn { get; set; }

    /// <summary>
    /// Tipo do crédito constituído (obrigatório, valores: ISSQN ou Outros)
    /// </summary>
    /// <example>ISSQN</example>
    public string TipoCredito { get; set; } = string.Empty;

    /// <summary>
    /// Indica se o contribuinte é optante pelo Simples Nacional (obrigatório, valores: Sim ou Não)
    /// </summary>
    /// <example>Sim</example>
    public string SimplesNacional { get; set; } = string.Empty;

    /// <summary>
    /// Alíquota aplicada no cálculo do ISSQN (obrigatório, de 0 a 100)
    /// </summary>
    /// <example>5.0</example>
    public decimal Aliquota { get; set; }

    /// <summary>
    /// Valor total faturado que serviu de base para o cálculo (obrigatório, maior que zero)
    /// </summary>
    /// <example>30000.00</example>
    public decimal ValorFaturado { get; set; }

    /// <summary>
    /// Valor de dedução aplicado sobre o valor faturado (obrigatório, maior ou igual a zero)
    /// </summary>
    /// <example>5000.00</example>
    public decimal ValorDeducao { get; set; }

    /// <summary>
    /// Base de cálculo do ISSQN (obrigatório, maior que zero, deve ser igual a ValorFaturado - ValorDeducao)
    /// </summary>
    /// <example>25000.00</example>
    public decimal BaseCalculo { get; set; }
}
