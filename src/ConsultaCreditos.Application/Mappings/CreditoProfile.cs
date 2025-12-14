using AutoMapper;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Enums;

namespace ConsultaCreditos.Application.Mappings;

public class CreditoProfile : Profile
{
    public CreditoProfile()
    {
        CreateMap<Credito, CreditoDto>()
            .ForMember(dest => dest.TipoCredito, opt => opt.MapFrom(src => src.TipoCredito.ToString()))
            .ForMember(dest => dest.SimplesNacional, opt => opt.MapFrom(src => src.SimplesNacional ? "Sim" : "Não"));

        CreateMap<IntegrarCreditoRequest, Credito>()
            .ConvertUsing(src => Credito.Criar(
                src.NumeroCredito,
                src.NumeroNfse,
                src.DataConstituicao,
                src.ValorIssqn,
                ParseTipoCredito(src.TipoCredito),
                ParseSimplesNacional(src.SimplesNacional),
                src.Aliquota,
                src.ValorFaturado,
                src.ValorDeducao,
                src.BaseCalculo
            ));
    }

    private static TipoCredito ParseTipoCredito(string tipo)
    {
        return tipo.ToUpper() switch
        {
            "ISSQN" => TipoCredito.ISSQN,
            "OUTROS" => TipoCredito.Outros,
            _ => throw new ArgumentException($"Tipo de crédito inválido: {tipo}")
        };
    }

    private static bool ParseSimplesNacional(string simplesNacional)
    {
        return simplesNacional.ToLower() switch
        {
            "sim" => true,
            "não" => false,
            _ => throw new ArgumentException($"Valor inválido para Simples Nacional: {simplesNacional}")
        };
    }
}
