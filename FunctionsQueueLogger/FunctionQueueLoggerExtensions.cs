using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QueueLogger;
using System;

namespace FunctionsQueueLogger
{
    public static class FunctionQueueLoggerExtensions
    {
        public static IServiceCollection AddQueueLogger(this IServiceCollection service)
        {
            var logger = new QueueLogger(new Microsoft.Azure.ServiceBus.QueueClient(
                    Environment.GetEnvironmentVariable("QueueLoggerConnectionString"),
                    Environment.GetEnvironmentVariable("QueueLoggerQueue")
                ));
            service.AddSingleton(logger);

            return service;
        }

        public static void Track(this ILogger logger, LogLevel level, string trackedId, object message, string sessionId = "", [System.Runtime.CompilerServices.CallerMemberName] string source = "")
        {
            QueueLogger.Instance?.Log(new TrackedMessage(trackedId, message)
            {
                SessionId = sessionId,
                TrackedId = trackedId,
                Level = level,
                Source = source,
                Type = "Serverless"
            });
        }
    }
}
