using Microsoft.Extensions.Logging;
using System;
using Microsoft.Azure.ServiceBus;

namespace QueueLogger
{
    public class QueueLogger : ILogger
    {
        public QueueLogger(QueueClient client, string source, LogLevel minLogLevel = LogLevel.Information)
        {
            this.client = client;
            this.source = source;
            this.minLevel = minLogLevel;
            Instance = this;
        }

        public QueueLogger(string connectionString, string queueName, string source) : this(new QueueClient(connectionString, queueName), source)
        {

        }

        public QueueLogger(IQueueLoggerOptions config) : this(new QueueClient(config.ConnectionString,config.Queue),config.Source)
        {
            this.minLevel = config.MinLogLevel;
        }

        private object currentState;
        private readonly QueueClient client;
        private string source;
        private LogLevel minLevel;
        internal static QueueLogger Instance;

        public IDisposable BeginScope<TState>(TState state)
        {
            var newState = new QueueLoggerState<TState>(state);
            currentState = newState;
            return newState;
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel>=minLevel;

        public void Log(TrackedMessage trackedMessage)
        {
            client?.SendAsync(trackedMessage.GetMessage());
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                var id = string.IsNullOrEmpty(eventId.Name) ? source : eventId.Name;

                Log(new TrackedMessage(id, formatter(state, exception))
                {
                    EventId = eventId.Id,
                    Level = logLevel,
                    Source = source,
                    Type = "Logger"
                });
            }
        }
    }
}
