using MediatR;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;
using CanopyManage.Common.Logger;

namespace CanopyManage.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                _logger.CustomLogInformation($"Handling {typeof(TRequest).Name}");
                var response = await next();
                _logger.CustomLogInformation($"Handled {typeof(TRequest).Name}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.CustomLogError(ex.Message, ex, payload: request);
                throw;
            }
        }
    }
}
