using AutoMapper;
using ConsultaCreditos.Application.Handlers;
using ConsultaCreditos.Application.Interfaces;
using ConsultaCreditos.Application.Mappings;
using ConsultaCreditos.Application.Queries;
using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Enums;
using ConsultaCreditos.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConsultaCreditos.UnitTests.Application.Handlers;

public class ObterCreditoPorNumeroHandlerTests
{
    private readonly Mock<ICreditoRepository> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly ObterCreditoPorNumeroHandler _handler;
    private readonly Mock<IServiceBusPublisher> _serviceBusPublisherMock;
    public ObterCreditoPorNumeroHandlerTests()
    {
        _repositoryMock = new Mock<ICreditoRepository>();
        _serviceBusPublisherMock = new Mock<IServiceBusPublisher>();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CreditoProfile>();
        });
        _mapper = configuration.CreateMapper();

        var loggerMock = new Mock<ILogger<ObterCreditoPorNumeroHandler>>();
        _handler = new ObterCreditoPorNumeroHandler(_repositoryMock.Object, _mapper, _serviceBusPublisherMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ComCreditoEncontrado_DeveRetornarCreditoDto()
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

        _repositoryMock.Setup(x => x.ObterPorNumeroCreditoAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(credito);

        var query = new ObterCreditoPorNumeroQuery("123456");

        var result = await _handler.Handle(query);

        result.Should().NotBeNull();
        result!.NumeroCredito.Should().Be("123456");
        result.NumeroNfse.Should().Be("7891011");
        result.ValorIssqn.Should().Be(1250m);
    }

    [Fact]
    public async Task Handle_SemCreditoEncontrado_DeveRetornarNull()
    {
        _repositoryMock.Setup(x => x.ObterPorNumeroCreditoAsync("999999", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Credito?)null);

        var query = new ObterCreditoPorNumeroQuery("999999");

        var result = await _handler.Handle(query);

        result.Should().BeNull();
    }
}
