using Microsoft.Extensions.Logging;
using System;
using Microsoft.Azure.ServiceBus;
using QueueLogger;

namespace FunctionsQueueLogger
{
    public class QueueLogger
    {
        public QueueLogger(QueueClient client)
        {
            this.client = client;
            Instance = this;
        }

        public QueueLogger(string connectionString, string queueName, string source) : this(new QueueClient(connectionString, queueName))
        {

        }

        public QueueLogger(QueueLoggerOptions config) : this(new QueueClient(config.ConnectionString, config.Queue))
        {
        }


        private readonly QueueClient client;
        internal static QueueLogger Instance;

        public void Log(TrackedMessage trackedMessage)
        {
            client?.SendAsync(trackedMessage.GetMessage());
        }

    }
}
