using AutoMapper;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Handlers;
using ConsultaCreditos.Application.Mappings;
using ConsultaCreditos.Application.Queries;
using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Enums;
using ConsultaCreditos.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ConsultaCreditos.UnitTests.Application.Handlers;

public class ObterCreditosPorNfseHandlerTests
{
    private readonly Mock<ICreditoRepository> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly ObterCreditosPorNfseHandler _handler;

    public ObterCreditosPorNfseHandlerTests()
    {
        _repositoryMock = new Mock<ICreditoRepository>();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CreditoProfile>();
        });
        _mapper = configuration.CreateMapper();

        _handler = new ObterCreditosPorNfseHandler(_repositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ComCreditosEncontrados_DeveRetornarListaDeCreditoDto()
    {
        var creditos = new List<Credito>
        {
            Credito.Criar(
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
            ),
            Credito.Criar(
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
            )
        };

        _repositoryMock.Setup(x => x.ObterPorNumeroNfseAsync("7891011", It.IsAny<CancellationToken>()))
            .ReturnsAsync(creditos);

        var query = new ObterCreditosPorNfseQuery("7891011");

        var result = await _handler.Handle(query);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].NumeroCredito.Should().Be("123456");
        result[1].NumeroCredito.Should().Be("789012");
    }

    [Fact]
    public async Task Handle_SemCreditosEncontrados_DeveRetornarListaVazia()
    {
        _repositoryMock.Setup(x => x.ObterPorNumeroNfseAsync("9999999", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Credito>());

        var query = new ObterCreditosPorNfseQuery("9999999");

        var result = await _handler.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
