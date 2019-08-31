using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;
using System.Collections.Concurrent;

namespace QueueLogger
{
    [ProviderAlias("Queue")]
    public class QueueLoggerProvider : ILoggerProvider
    {
        public QueueLoggerProvider()
        {

        }

        public QueueLoggerProvider(QueueLoggerOptions settings)
        {
            if (settings.Source == null || settings.Source=="Unknown")
            {
                settings.Source = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            }
            this.settings = settings;
        }

        private readonly ConcurrentDictionary<string, QueueLogger> _loggers = new ConcurrentDictionary<string, QueueLogger>();
        private readonly IQueueLoggerOptions settings;

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, new QueueLogger(settings));
        }

        public void Dispose()
        {

        }
    }
}
