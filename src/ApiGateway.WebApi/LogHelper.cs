using System;
using ApiGateway.Common.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace ApiGateway.WebApi
{
    public static class LogHelper
    {
        public static LogSettings Settings
        {
            get;
            private set;
        }

        public static void Init(LogSettings logSettings)
        {
            Settings = logSettings;

            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ServiceName", logSettings.ServiceName);

            if (logSettings.UseElasticSearch)
            {
                loggerConfig.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(logSettings.ElasticSearchUrl))
                {
                    AutoRegisterTemplate = true,
                    MinimumLogEventLevel = (LogEventLevel)logSettings.LogEventLevel,
                    CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                    IndexFormat = logSettings.IndexNamePrefix + "-{0:yyyy.MM.dd}"
                });
            }
            else
            {
                loggerConfig.WriteTo.LiterateConsole((LogEventLevel)logSettings.LogEventLevel);
            }


            Log.Logger = loggerConfig.CreateLogger();    
        }
    }
}