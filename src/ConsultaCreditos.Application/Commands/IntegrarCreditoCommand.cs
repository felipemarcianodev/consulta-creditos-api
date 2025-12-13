using ConsultaCreditos.Application.DTOs;

namespace ConsultaCreditos.Application.Commands;

public class IntegrarCreditoCommand
{
    public List<IntegrarCreditoRequest> Creditos { get; set; } = new();

    public IntegrarCreditoCommand(List<IntegrarCreditoRequest> creditos)
    {
        Creditos = creditos;
    }
}
