using AutoMapper;
using ConsultaCreditos.Application.Commands;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Handlers;
using ConsultaCreditos.Application.Mappings;
using ConsultaCreditos.Domain.Entities;
using ConsultaCreditos.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConsultaCreditos.UnitTests.Application.Handlers;

public class ProcessarCreditoHandlerTests
{
    private readonly Mock<ICreditoRepository> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ProcessarCreditoHandler>> _loggerMock;
    private readonly ProcessarCreditoHandler _handler;

    public ProcessarCreditoHandlerTests()
    {
        _repositoryMock = new Mock<ICreditoRepository>();
        _loggerMock = new Mock<ILogger<ProcessarCreditoHandler>>();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CreditoProfile>();
        });
        _mapper = configuration.CreateMapper();

        _handler = new ProcessarCreditoHandler(_repositoryMock.Object, _mapper, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ComCreditoNovo_DeveAdicionarNoRepositorio()
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

        var command = new ProcessarCreditoCommand(request);

        _repositoryMock.Setup(x => x.ExisteAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await _handler.Handle(command);

        _repositoryMock.Verify(x => x.AdicionarAsync(It.IsAny<Credito>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ComCreditoExistente_NaoDeveAdicionarNoRepositorio()
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

        var command = new ProcessarCreditoCommand(request);

        _repositoryMock.Setup(x => x.ExisteAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await _handler.Handle(command);

        _repositoryMock.Verify(x => x.AdicionarAsync(It.IsAny<Credito>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ComCreditoExistente_DeveLogarIdempotencia()
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

        var command = new ProcessarCreditoCommand(request);

        _repositoryMock.Setup(x => x.ExisteAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await _handler.Handle(command);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("já existe")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ComCreditoNovo_DeveLogarSucesso()
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

        var command = new ProcessarCreditoCommand(request);

        _repositoryMock.Setup(x => x.ExisteAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await _handler.Handle(command);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("processado e persistido")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
