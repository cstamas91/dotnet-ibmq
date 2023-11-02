using CsT.Dotnet.IbmQ.Core;
using Serilog;
using Microsoft.Extensions.Logging;

namespace CsT.Dotnet.IbmQ.Tests;

public class MQTests
{
    [Fact]
    public void MQ_Connects()
    {
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        var consumer = new Core.HelloConsumer(logger);
    }
}