using ConsultaCreditos.Application.DTOs;

namespace ConsultaCreditos.Application.Commands;

public class ProcessarCreditoCommand
{
    public IntegrarCreditoRequest Credito { get; set; }

    public ProcessarCreditoCommand(IntegrarCreditoRequest credito)
    {
        Credito = credito;
    }
}
