using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Options;

namespace CanopyManage.Common.Logger
{
    public static class LoggerInjector
    {
        public static IServiceCollection RegisterLogger(this IServiceCollection services, string instrumentationKey, LogLevel logLevel = LogLevel.Information)
        {
            services.AddLogging();

            var filterOptions = new LoggerFilterOptions();
            filterOptions.AddFilter(string.Empty, logLevel);
            var config = TelemetryConfiguration.CreateDefault();
            config.InstrumentationKey = instrumentationKey;

            IOptions<TelemetryConfiguration> telemeryOptions = Options.Create(config);
            IOptions<ApplicationInsightsLoggerOptions> configureApplicationInsightsLoggerOptions = Options.Create(
                new ApplicationInsightsLoggerOptions());

            ILoggerFactory loggerFactory =
               new LoggerFactory(
                   new[]
                   {
                        new ApplicationInsightsLoggerProvider(telemeryOptions,
                            configureApplicationInsightsLoggerOptions)
                   }, filterOptions);

            services.AddSingleton<ILoggerFactory>(loggerFactory);

            return services;
        }
    }
}
