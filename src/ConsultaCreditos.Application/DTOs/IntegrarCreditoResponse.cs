namespace ConsultaCreditos.Application.DTOs;

public class IntegrarCreditoResponse
{
    public bool Success { get; set; }

    public static IntegrarCreditoResponse Sucesso() => new() { Success = true };
}
