using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace QueueLogger
{

    public static class QueueLoggerExtensions
    {
        public static void QueueLog(this ILogger logger, TrackedMessage message)
        {
            if (QueueLogger.Instance!=null)
            {
                QueueLogger.Instance.Log(message);
            }
            else
            {
                logger.LogWarning("QueueLogger not started, please check configuration");
            }
        }


        public static ILoggingBuilder AddQueue(this ILoggingBuilder builder, string connectionString, string queue, string source, LogLevel minLevel = LogLevel.Information)
        {
            return builder.AddQueue(new QueueLoggerOptions()
            {
                ConnectionString = connectionString,
                Queue = queue,
                Source = source,
            });
        }

        public static ILoggingBuilder AddQueue(this ILoggingBuilder builder, Action<QueueLoggerOptions> settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var config = new QueueLoggerOptions();
            settings(config);

            return builder.AddQueue(config);
        }

        public static ILoggingBuilder AddQueue(this ILoggingBuilder builder, QueueLoggerOptions config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            builder.Services.AddSingleton(config);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, QueueLoggerProvider>());

            return builder;
        }

        public static ILoggerFactory AddQueue(
            this ILoggerFactory factory,
            QueueLoggerOptions settings)
        {
            factory.AddProvider(new QueueLoggerProvider(settings));
            return factory;
        }


        public static ILoggerFactory AddQueue(this ILoggerFactory factory, Action<QueueLoggerOptions> settings)
        {
            var config = new QueueLoggerOptions() { };

            settings(config);

            factory.AddProvider(new QueueLoggerProvider(config));
            return factory;
        }

    }
}
