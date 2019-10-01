using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QueueLogger;
using System;

namespace FunctionsQueueLogger
{
    public static class FunctionQueueLoggerExtensions
    {
        public static IServiceCollection AddQueueLogger(this IServiceCollection service, string globalType = "Serverless")
        {
            var logger = new QueueLogger(new Microsoft.Azure.ServiceBus.QueueClient(
                    Environment.GetEnvironmentVariable("QueueLoggerConnectionString"),
                    Environment.GetEnvironmentVariable("QueueLoggerQueue")),
                    globalType);

            service.AddSingleton(logger);

            return service;
        }

        public static void Track(this ILogger logger, LogLevel level, string trackedId, object message, [System.Runtime.CompilerServices.CallerMemberName] string source = "", string type = "")
        {
            if (string.IsNullOrEmpty(type))
                type = QueueLogger.Instance?.GlobalType;
            QueueLogger.Instance?.Log(new TrackedMessage(trackedId, message)
            {
                TrackedId = trackedId,
                Level = level,
                Source = source,
                Type = type
            });
        }
    }
}
