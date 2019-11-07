
namespace QueueLogger
{
    public interface IQueueLoggerOptions
    {
        string ConnectionString { get; }
        string Queue { get; }
        string Source { get; }
    }
}