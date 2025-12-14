namespace ConsultaCreditos.Application.DTOs;

/// <summary>
/// Resposta da operação de integração de créditos
/// </summary>
public class IntegrarCreditoResponse
{
    /// <summary>
    /// Indica se a operação foi concluída com sucesso
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Cria uma resposta de sucesso
    /// </summary>
    /// <returns>Resposta indicando sucesso na operação</returns>
    public static IntegrarCreditoResponse Sucesso() => new() { Success = true };
}
