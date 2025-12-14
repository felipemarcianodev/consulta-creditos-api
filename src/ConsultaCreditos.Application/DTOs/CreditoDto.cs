namespace ConsultaCreditos.Application.DTOs;

/// <summary>
/// Dados de um crédito constituído
/// </summary>
public class CreditoDto
{
    /// <summary>
    /// Número identificador do crédito constituído
    /// </summary>
    /// <example>123456</example>
    public string NumeroCredito { get; set; } = string.Empty;

    /// <summary>
    /// Número da Nota Fiscal de Serviços Eletrônica
    /// </summary>
    /// <example>7891011</example>
    public string NumeroNfse { get; set; } = string.Empty;

    /// <summary>
    /// Data em que o crédito foi constituído
    /// </summary>
    /// <example>2024-02-25</example>
    public DateTime DataConstituicao { get; set; }

    /// <summary>
    /// Valor do Imposto Sobre Serviços de Qualquer Natureza
    /// </summary>
    /// <example>1500.75</example>
    public decimal ValorIssqn { get; set; }

    /// <summary>
    /// Tipo do crédito constituído (ISSQN ou Outros)
    /// </summary>
    /// <example>ISSQN</example>
    public string TipoCredito { get; set; } = string.Empty;

    /// <summary>
    /// Indica se o contribuinte é optante pelo Simples Nacional (Sim ou Não)
    /// </summary>
    /// <example>Sim</example>
    public string SimplesNacional { get; set; } = string.Empty;

    /// <summary>
    /// Alíquota aplicada no cálculo do ISSQN (percentual de 0 a 100)
    /// </summary>
    /// <example>5.0</example>
    public decimal Aliquota { get; set; }

    /// <summary>
    /// Valor total faturado que serviu de base para o cálculo
    /// </summary>
    /// <example>30000.00</example>
    public decimal ValorFaturado { get; set; }

    /// <summary>
    /// Valor de dedução aplicado sobre o valor faturado
    /// </summary>
    /// <example>5000.00</example>
    public decimal ValorDeducao { get; set; }

    /// <summary>
    /// Base de cálculo do ISSQN (Valor Faturado - Valor Dedução)
    /// </summary>
    /// <example>25000.00</example>
    public decimal BaseCalculo { get; set; }
}
