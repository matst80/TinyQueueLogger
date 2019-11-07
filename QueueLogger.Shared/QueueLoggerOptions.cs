
namespace QueueLogger
{
    public class QueueLoggerOptions : IQueueLoggerOptions
    {
        public string ConnectionString { get; set; }
        public string Queue { get; set; }
        public string Source { get; set; } = "Unknown";
    }
}
