using System;

namespace ConsultaCreditos.Application.DTOs;

public class AuditoriaConsultaEvent
{
    public string TipoConsulta { get; set; }
    public string NumeroReferencia { get; set; } // Pode ser NumeroNfse ou NumeroCredito
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
    public string? Detalhes { get; set; } // Informações adicionais, se houver
}
