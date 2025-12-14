using AutoMapper;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Mappings;
using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Enums;
using FluentAssertions;

namespace ConsultaCreditos.UnitTests.Application.Mappings;

public class CreditoProfileTests
{
    private readonly IMapper _mapper;

    public CreditoProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CreditoProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void Map_CreditoParaCreditoDto_DeveMappearCorretamente()
    {
        var credito = Credito.Criar(
            numeroCredito: "123456",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 25),
            valorIssqn: 1250m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: true,
            aliquota: 5m,
            valorFaturado: 30000m,
            valorDeducao: 5000m,
            baseCalculo: 25000m
        );

        var dto = _mapper.Map<CreditoDto>(credito);

        dto.Should().NotBeNull();
        dto.NumeroCredito.Should().Be("123456");
        dto.NumeroNfse.Should().Be("7891011");
        dto.DataConstituicao.Should().Be(new DateTime(2024, 2, 25));
        dto.ValorIssqn.Should().Be(1250m);
        dto.TipoCredito.Should().Be("ISSQN");
        dto.SimplesNacional.Should().Be("Sim");
        dto.Aliquota.Should().Be(5m);
        dto.ValorFaturado.Should().Be(30000m);
        dto.ValorDeducao.Should().Be(5000m);
        dto.BaseCalculo.Should().Be(25000m);
    }

    [Fact]
    public void Map_CreditoParaCreditoDto_ComSimplesNacionalFalso_DeveMappearParaNao()
    {
        var credito = Credito.Criar(
            numeroCredito: "789012",
            numeroNfse: "7891011",
            dataConstituicao: new DateTime(2024, 2, 26),
            valorIssqn: 945m,
            tipoCredito: TipoCredito.ISSQN,
            simplesNacional: false,
            aliquota: 4.5m,
            valorFaturado: 25000m,
            valorDeducao: 4000m,
            baseCalculo: 21000m
        );

        var dto = _mapper.Map<CreditoDto>(credito);

        dto.SimplesNacional.Should().Be("Não");
    }

    [Fact]
    public void Map_CreditoParaCreditoDto_ComTipoCreditoOutros_DeveMappearCorretamente()
    {
        var credito = Credito.Criar(
            numeroCredito: "654321",
            numeroNfse: "1122334",
            dataConstituicao: new DateTime(2024, 1, 15),
            valorIssqn: 595m,
            tipoCredito: TipoCredito.Outros,
            simplesNacional: true,
            aliquota: 3.5m,
            valorFaturado: 20000m,
            valorDeducao: 3000m,
            baseCalculo: 17000m
        );

        var dto = _mapper.Map<CreditoDto>(credito);

        dto.TipoCredito.Should().Be("Outros");
    }

    [Fact]
    public void Map_IntegrarCreditoRequestParaCredito_DeveMappearCorretamente()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "123456",
            NumeroNfse = "7891011",
            DataConstituicao = new DateTime(2024, 2, 25),
            ValorIssqn = 1250m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Sim",
            Aliquota = 5m,
            ValorFaturado = 30000m,
            ValorDeducao = 5000m,
            BaseCalculo = 25000m
        };

        var credito = _mapper.Map<Credito>(request);

        credito.Should().NotBeNull();
        credito.NumeroCredito.Should().Be("123456");
        credito.NumeroNfse.Should().Be("7891011");
        credito.DataConstituicao.Should().Be(new DateTime(2024, 2, 25));
        credito.ValorIssqn.Should().Be(1250m);
        credito.TipoCredito.Should().Be(TipoCredito.ISSQN);
        credito.SimplesNacional.Should().BeTrue();
        credito.Aliquota.Should().Be(5m);
        credito.ValorFaturado.Should().Be(30000m);
        credito.ValorDeducao.Should().Be(5000m);
        credito.BaseCalculo.Should().Be(25000m);
    }

    [Fact]
    public void Map_IntegrarCreditoRequestParaCredito_ComSimplesNacionalNao_DeveMappearParaFalse()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "789012",
            NumeroNfse = "7891011",
            DataConstituicao = new DateTime(2024, 2, 26),
            ValorIssqn = 945m,
            TipoCredito = "ISSQN",
            SimplesNacional = "Não",
            Aliquota = 4.5m,
            ValorFaturado = 25000m,
            ValorDeducao = 4000m,
            BaseCalculo = 21000m
        };

        var credito = _mapper.Map<Credito>(request);

        credito.SimplesNacional.Should().BeFalse();
    }

    [Fact]
    public void Map_IntegrarCreditoRequestParaCredito_ComTipoCreditoOutros_DeveMappearCorretamente()
    {
        var request = new IntegrarCreditoRequest
        {
            NumeroCredito = "654321",
            NumeroNfse = "1122334",
            DataConstituicao = new DateTime(2024, 1, 15),
            ValorIssqn = 595m,
            TipoCredito = "Outros",
            SimplesNacional = "Sim",
            Aliquota = 3.5m,
            ValorFaturado = 20000m,
            ValorDeducao = 3000m,
            BaseCalculo = 17000m
        };

        var credito = _mapper.Map<Credito>(request);

        credito.TipoCredito.Should().Be(TipoCredito.Outros);
    }

    [Fact]
    public void ConfigurationIsValid_DeveRetornarSucesso()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CreditoProfile>();
        });

        configuration.AssertConfigurationIsValid();
    }
}
