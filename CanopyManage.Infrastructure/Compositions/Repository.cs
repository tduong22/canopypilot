using CanopyManage.Domain.Aggregates;
using CanopyManage.Domain.SeedWork;
using CanopyManage.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CanopyManage.Infrastructure.Compositions
{
    public static class RepositoryComposition
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            //inject client

            services.AddScoped<IRepository<AzureResource<ServiceNowServiceAccount>, string>, ServiceAccountKeyVaultRepository>();

            return services;
        }
    }
}
