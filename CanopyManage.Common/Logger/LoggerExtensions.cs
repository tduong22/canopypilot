using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CanopyManage.Common.Logger
{
    public static class LoggerExtensions
    {
        private const string CorrelationKey = "CorrelationId";
        private const string PayloadKey = "Payload";

        public static void CustomLogDebug(this ILogger logger, string message, Exception exception = null, string correlationId = "", object payload = null)
        {
            Guard.Against.Null(logger, nameof(logger));
            logger.CustomLog(LogLevel.Debug, message, exception, correlationId, payload);
        }

        public static void CustomLogTrace(this ILogger logger, string message, Exception exception = null, string correlationId = "", object payload = null)
        {
            Guard.Against.Null(logger, nameof(logger));
            logger.CustomLog(LogLevel.Trace, message, exception, correlationId, payload);
        }

        public static void CustomLogInformation(this ILogger logger, string message, Exception exception = null, string correlationId = "", object payload = null)
        {
            Guard.Against.Null(logger, nameof(logger));
            logger.CustomLog(LogLevel.Information, message, exception, correlationId, payload);
        }

        public static void CustomLogWarning(this ILogger logger, string message, Exception exception = null, string correlationId = "", object payload = null)
        {
            Guard.Against.Null(logger, nameof(logger));
            logger.CustomLog(LogLevel.Warning, message, exception, correlationId, payload);
        }

        public static void CustomLogError(this ILogger logger, string message, Exception exception = null, string correlationId = "", object payload = null)
        {
            Guard.Against.Null(logger, nameof(logger));
            logger.CustomLog(LogLevel.Error, message, exception, correlationId, payload);
        }

        public static void CustomLogCritical(this ILogger logger, string message, Exception exception = null, string correlationId = "", object payload = null)
        {
            Guard.Against.Null(logger, nameof(logger));
            logger.CustomLog(LogLevel.Critical, message, exception, correlationId, payload);
        }

        private static void CustomLog(this ILogger logger, LogLevel logLevel, string message, Exception exception = null, string correlationId = "", object payload = null)
        {
            Guard.Against.Null(logger, nameof(logger));
            var customProperties = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(correlationId))
            {
                customProperties.Add(CorrelationKey, correlationId);
            }

            if (payload != null)
            {
                string payloadJson;
                try
                {
                    payloadJson = JsonConvert.SerializeObject(payload);
                }
                catch
                {
                    payloadJson = "Exception occurred while deserialize object to Json.";
                }
                customProperties.Add(PayloadKey, payloadJson);
            }

            logger.Log(logLevel, new EventId(9999),
               customProperties
               , exception,
               (s, ex) =>
                   message);
        }
    }
}
