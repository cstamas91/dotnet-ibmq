namespace CsT.Dotnet.IbmQ.Core;

using IBM.XMS;

public class HelloConsumer : BackgroundService
{
    private readonly ILogger<HelloConsumer> _logger;
    public HelloConsumer(ILogger<HelloConsumer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
