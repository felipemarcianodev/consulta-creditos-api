using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ConsultaCreditos.Application.DTOs;
using ConsultaCreditos.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsultaCreditos.Infrastructure.Messaging;

[ExcludeFromCodeCoverage]
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

    public async Task PublishAsync<T>(T credito, CancellationToken cancellationToken = default) where T: class
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
                "Publicado no Service Bus com sucesso. MessageId: {MessageId}",
                message.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar no Service Bus");
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _sender.DisposeAsync();
        await _client.DisposeAsync();
    }
}
