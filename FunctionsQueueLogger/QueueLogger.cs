using Microsoft.Extensions.Logging;
using System;
using Microsoft.Azure.ServiceBus;
using QueueLogger;

namespace FunctionsQueueLogger
{
    public class QueueLogger
    {
        public QueueLogger(QueueClient client, string type)
        {
            this.client = client;
            Instance = this;
            GlobalType = type;
        }

        public QueueLogger(string connectionString, string queueName, string source, string type) : this(new QueueClient(connectionString, queueName), type)
        {

        }

        public QueueLogger(QueueLoggerOptions config, string type) : this(new QueueClient(config.ConnectionString, config.Queue), type)
        {
        }

        public string GlobalType { get; set; }

        private readonly QueueClient client;
        internal static QueueLogger Instance;

        public void Log(TrackedMessage trackedMessage)
        {
            client?.SendAsync(trackedMessage.GetMessage());
        }

    }
}
