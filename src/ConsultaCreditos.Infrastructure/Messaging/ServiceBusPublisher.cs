using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsultaCreditos.Infrastructure.Messaging;

public class ServiceBusPublisher : IServiceBusPublisher
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;
    private readonly ILogger<ServiceBusPublisher> _logger;

    public ServiceBusPublisher(IConfiguration configuration, ILogger<ServiceBusPublisher> logger)
    {
        _logger = logger;

        var connectionString = configuration["ServiceBus:ConnectionString"]
            ?? throw new InvalidOperationException("ServiceBus ConnectionString não configurada");

        var topicName = configuration["ServiceBus:TopicName"]
            ?? throw new InvalidOperationException("ServiceBus TopicName não configurado");

        _client = new ServiceBusClient(connectionString);
        _sender = _client.CreateSender(topicName);
    }

    public async Task PublicarCreditoAsync(IntegrarCreditoRequest credito, CancellationToken cancellationToken = default)
    {
        try
        {
            var messageBody = JsonSerializer.Serialize(credito);
            var message = new ServiceBusMessage(messageBody)
            {
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "application/json"
            };

            await _sender.SendMessageAsync(message, cancellationToken);

            _logger.LogInformation(
                "Crédito {NumeroCredito} publicado no Service Bus com sucesso. MessageId: {MessageId}",
                credito.NumeroCredito,
                message.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar crédito {NumeroCredito} no Service Bus", credito.NumeroCredito);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _sender.DisposeAsync();
        await _client.DisposeAsync();
    }
}
