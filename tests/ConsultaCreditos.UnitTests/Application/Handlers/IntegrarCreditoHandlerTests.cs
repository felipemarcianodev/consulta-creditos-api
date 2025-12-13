using ConsultaCreditos.Application.Commands;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Handlers;
using ConsultaCreditos.Application.Interfaces;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace ConsultaCreditos.UnitTests.Application.Handlers;

public class IntegrarCreditoHandlerTests
{
    private readonly Mock<IServiceBusPublisher> _serviceBusPublisherMock;
    private readonly IntegrarCreditoHandler _handler;

    public IntegrarCreditoHandlerTests()
    {
        _serviceBusPublisherMock = new Mock<IServiceBusPublisher>();
        _handler = new IntegrarCreditoHandler(_serviceBusPublisherMock.Object);
    }

    [Fact]
    public async Task Handle_ComCreditosValidos_DevePublicarNoServiceBus()
    {
        var creditos = new List<IntegrarCreditoRequest>
        {
            new()
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
            },
            new()
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
            }
        };

        var command = new IntegrarCreditoCommand(creditos);

        var result = await _handler.Handle(command);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        _serviceBusPublisherMock.Verify(x => x.PublicarCreditoAsync(It.IsAny<IntegrarCreditoRequest>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_ComCreditoInvalido_DeveLancarValidationException()
    {
        var creditos = new List<IntegrarCreditoRequest>
        {
            new()
            {
                NumeroCredito = "",
                NumeroNfse = "7891011",
                DataConstituicao = new DateTime(2024, 2, 25),
                ValorIssqn = 1250m,
                TipoCredito = "ISSQN",
                SimplesNacional = "Sim",
                Aliquota = 5m,
                ValorFaturado = 30000m,
                ValorDeducao = 5000m,
                BaseCalculo = 25000m
            }
        };

        var command = new IntegrarCreditoCommand(creditos);

        var act = async () => await _handler.Handle(command);

        await act.Should().ThrowAsync<ValidationException>();
        _serviceBusPublisherMock.Verify(x => x.PublicarCreditoAsync(It.IsAny<IntegrarCreditoRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ComListaVazia_DeveRetornarSucesso()
    {
        var creditos = new List<IntegrarCreditoRequest>();
        var command = new IntegrarCreditoCommand(creditos);

        var result = await _handler.Handle(command);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        _serviceBusPublisherMock.Verify(x => x.PublicarCreditoAsync(It.IsAny<IntegrarCreditoRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ComCreditoValido_DevePublicarCadaCreditoIndividualmente()
    {
        var credito1 = new IntegrarCreditoRequest
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

        var credito2 = new IntegrarCreditoRequest
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

        var creditos = new List<IntegrarCreditoRequest> { credito1, credito2 };
        var command = new IntegrarCreditoCommand(creditos);

        await _handler.Handle(command);

        _serviceBusPublisherMock.Verify(x => x.PublicarCreditoAsync(credito1, It.IsAny<CancellationToken>()), Times.Once);
        _serviceBusPublisherMock.Verify(x => x.PublicarCreditoAsync(credito2, It.IsAny<CancellationToken>()), Times.Once);
    }
}
