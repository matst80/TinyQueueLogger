using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QueueLogger;

namespace FunctionsQueueLogger
{
    public static class FunctionQueueLoggerExtensions
    {
        public static IServiceCollection AddQueueLogger(this IServiceCollection service, string connectionStringKey, string topicKey, string globalType = "Serverless")
        {
            var logger = new QueueLogger(new Microsoft.Azure.ServiceBus.QueueClient(
                    Environment.GetEnvironmentVariable(connectionStringKey),
                    Environment.GetEnvironmentVariable(topicKey)),
                globalType);

            service.AddSingleton(logger);

            return service;
        }

        public static void Track(this ILogger log, LogLevel level, string trackedId, string title, object message, string sessionId = "", string type = "", [System.Runtime.CompilerServices.CallerMemberName] string source = "")
        {
            if (string.IsNullOrEmpty(type))
                type = QueueLogger.Instance?.GlobalType;
            QueueLogger.Instance?.Log(new TrackedMessage(trackedId, message)
            {
                SessionId = sessionId,
                Title = title,
                TrackedId = trackedId,
                Level = level.ToString(),
                Source = source,
                Type = type
            });
            if (message != null)
            {
                if (message is Exception exception)
                {
                    log?.LogError(new EventId(1, sessionId ?? "nosessing"), exception, source);
                }
                else
                {
                    log?.Log(level, $"{message?.ToString() ?? "No message supplied"} {sessionId ?? ""} {trackedId ?? ""}");
                }
            }
        }
    }
}