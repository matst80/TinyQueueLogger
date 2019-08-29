using Microsoft.Extensions.Logging;

namespace QueueLogger
{
    public interface IQueueLoggerOptions
    {
        string ConnectionString { get; }
        string Queue { get; }
        LogLevel MinLogLevel { get; }
        string Source { get; }
    }
}