using CsT.Dotnet.IbmQ.Core;

namespace CsT.Dotnet.IbmQ.Worker;

public class HelloConsumerWorker : BackgroundService
{
    private readonly ILogger<HelloConsumerWorker> _logger;
    private readonly HelloConsumer _consumer;
    public HelloConsumerWorker(
        ILogger<HelloConsumerWorker> logger,
        HelloConsumer consumer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _consumer = consumer;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.ReceiveMessages(stoppingToken);
        await Task.CompletedTask;
    }
}
