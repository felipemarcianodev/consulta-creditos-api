using ConsultaCreditos.Infrastructure.Messaging;

namespace ConsultaCreditos.API.BackgroundServices;

public class CreditoProcessorService : BackgroundService
{
    private readonly ILogger<CreditoProcessorService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly int _checkIntervalMs;

    public CreditoProcessorService(
        ILogger<CreditoProcessorService> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _checkIntervalMs = configuration.GetValue<int>("ServiceBus:CheckInterval", 500);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CreditoProcessorService iniciado. Intervalo de verificação: {Interval}ms", _checkIntervalMs);

        await Task.Delay(5000, stoppingToken);

        using var scope = _serviceProvider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<ServiceBusConsumer>();

        try
        {
            await consumer.StartAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_checkIntervalMs, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("CreditoProcessorService cancelado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro crítico no CreditoProcessorService");
            throw;
        }
        finally
        {
            await consumer.StopAsync(stoppingToken);
            _logger.LogInformation("CreditoProcessorService parado");
        }
    }
}
