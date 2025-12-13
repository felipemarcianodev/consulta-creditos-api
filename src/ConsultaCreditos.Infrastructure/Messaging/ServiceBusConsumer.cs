using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ConsultaCreditos.Application.Commands;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsultaCreditos.Infrastructure.Messaging;

public class ServiceBusConsumer
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly ProcessarCreditoHandler _handler;
    private readonly ILogger<ServiceBusConsumer> _logger;

    public ServiceBusConsumer(
        IConfiguration configuration,
        ProcessarCreditoHandler handler,
        ILogger<ServiceBusConsumer> logger)
    {
        _handler = handler;
        _logger = logger;

        var connectionString = configuration["ServiceBus:ConnectionString"]
            ?? throw new InvalidOperationException("ServiceBus ConnectionString não configurada");

        var topicName = configuration["ServiceBus:TopicName"]
            ?? throw new InvalidOperationException("ServiceBus TopicName não configurado");

        var subscriptionName = configuration["ServiceBus:SubscriptionName"]
            ?? throw new InvalidOperationException("ServiceBus SubscriptionName não configurado");

        _client = new ServiceBusClient(connectionString);

        var processorOptions = new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 1,
            AutoCompleteMessages = false
        };

        _processor = _client.CreateProcessor(topicName, subscriptionName, processorOptions);
        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _processor.StartProcessingAsync(cancellationToken);
        _logger.LogInformation("ServiceBusConsumer iniciado");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
        _logger.LogInformation("ServiceBusConsumer parado");
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        try
        {
            var messageBody = args.Message.Body.ToString();
            var credito = JsonSerializer.Deserialize<IntegrarCreditoRequest>(messageBody);

            if (credito == null)
            {
                _logger.LogWarning("Mensagem inválida recebida. MessageId: {MessageId}", args.Message.MessageId);
                await args.DeadLetterMessageAsync(args.Message, "InvalidMessage", "Falha ao deserializar mensagem");
                return;
            }

            _logger.LogInformation(
                "Processando crédito {NumeroCredito}. MessageId: {MessageId}",
                credito.NumeroCredito,
                args.Message.MessageId);

            var command = new ProcessarCreditoCommand(credito);
            await _handler.Handle(command, args.CancellationToken);

            await args.CompleteMessageAsync(args.Message);

            _logger.LogInformation(
                "Crédito {NumeroCredito} processado com sucesso. MessageId: {MessageId}",
                credito.NumeroCredito,
                args.Message.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar mensagem. MessageId: {MessageId}", args.Message.MessageId);
            await args.AbandonMessageAsync(args.Message);
        }
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(
            args.Exception,
            "Erro no ServiceBusProcessor. Source: {ErrorSource}",
            args.ErrorSource);

        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _processor.DisposeAsync();
        await _client.DisposeAsync();
    }
}
