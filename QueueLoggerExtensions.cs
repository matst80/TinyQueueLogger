using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace QueueLogger
{

    public static class QueueLoggerExtensions
    {
        public static ILoggingBuilder AddQueue(this ILoggingBuilder builder, string connectionString, string queue, string source, LogLevel minLevel = LogLevel.Information)
        {
            return builder.AddQueue(new QueueLoggerSettings()
            {
                ConnectionString = connectionString,
                Queue = queue,
                Source = source,
                MinLogLevel = minLevel
            });
        }

        public static ILoggingBuilder AddQueue(this ILoggingBuilder builder, Action<QueueLoggerSettings> settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var config = new QueueLoggerSettings();
            settings(config);

            return builder.AddQueue(config);
        }

        public static ILoggingBuilder AddQueue(this ILoggingBuilder builder, QueueLoggerSettings config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            builder.Services.AddSingleton(config);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, QueueLoggerProvider>());

            return builder;
        }


        public static ILoggerFactory AddQueue(this ILoggerFactory factory, Action<QueueLoggerSettings> settings)
        {
            var config = new QueueLoggerSettings() { };

            settings(config);

            factory.AddProvider(new QueueLoggerProvider(config));
            return factory;
        }

    }
}
