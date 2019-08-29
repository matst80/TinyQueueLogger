using Microsoft.Extensions.Logging;

namespace QueueLogger
{
    public class QueueLoggerSettings
    {
        public string ConnectionString { get; set; }
        public string Queue { get; set; }
        public LogLevel MinLogLevel { get; set; } = LogLevel.Information;
        public string Source { get; set; } = "Unknown";
    }
}
