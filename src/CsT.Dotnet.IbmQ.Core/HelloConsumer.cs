namespace CsT.Dotnet.IbmQ.Core;

using Microsoft.Extensions.Logging;

public class HelloConsumer
{
    private readonly ILogger<HelloConsumer> _logger;
    private readonly Env _env = new();
    private const int TIMEOUTTIME = 30000;

    private XMSFactoryFactory _factoryFactory;
    private IConnectionFactory _connectionFactory;
    private IConnection _connection;
    private ISession _session;
    private IDestination _destination;
    private IMessageConsumer _subscriber;

    public HelloConsumer(ILogger<HelloConsumer> logger)
    {
        _logger = logger;

        _factoryFactory = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ);
        _connectionFactory = _factoryFactory.CreateConnectionFactory();
        ConnectionPropertyBuilder.SetConnectionProperties(_connectionFactory, _env);
        _connection = _connectionFactory.CreateConnection();
        _session = _connection.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
        _destination = _session.CreateTopic(_env.Conn?.topic_name ?? "topic");
        _subscriber = _session.CreateConsumer(_destination);
    }

    public void ReceiveMessages(CancellationToken ct)
    {
        _connection.Start();

        while (!ct.IsCancellationRequested)
        {
            ITextMessage textMessage = (ITextMessage)_subscriber.Receive(TIMEOUTTIME);       

            if (textMessage != null)
            {
                Console.WriteLine($"Message received: {textMessage}");
            }
        }

        _subscriber.Close();

        _destination.Dispose();

        _session.Dispose();

        _connection.Close();
    }
}
