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
            .ForMember(dest => dest.NumeroCredito, opt => opt.MapFrom(src => src.NumeroCredito.Valor))
            .ForMember(dest => dest.NumeroNfse, opt => opt.MapFrom(src => src.NumeroNfse.Valor))
            .ForMember(dest => dest.DataConstituicao, opt => opt.MapFrom(src => src.DataConstituicao))
            .ForMember(dest => dest.ValorIssqn, opt => opt.MapFrom(src => src.ValorIssqn.Valor))
            .ForMember(dest => dest.TipoCredito, opt => opt.MapFrom(src => src.TipoCredito.ToString()))
            .ForMember(dest => dest.SimplesNacional, opt => opt.MapFrom(src => src.SimplesNacional ? "Sim" : "Não"))
            .ForMember(dest => dest.Aliquota, opt => opt.MapFrom(src => src.Aliquota.Valor))
            .ForMember(dest => dest.ValorFaturado, opt => opt.MapFrom(src => src.ValorFaturado.Valor))
            .ForMember(dest => dest.ValorDeducao, opt => opt.MapFrom(src => src.ValorDeducao.Valor))
            .ForMember(dest => dest.BaseCalculo, opt => opt.MapFrom(src => src.BaseCalculo.Valor));

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
