using CanopyManage.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CanopyManage.Application.Compositions
{
    public static class MediatorComposition
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            var assembly = typeof(LoggingBehavior<,>).Assembly;
            services.AddMediatR(assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

            return services;
        }
    }
}
