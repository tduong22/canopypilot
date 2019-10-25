using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace CanopyManage.IncidentService.Compositions
{
    public static class SwaggerComposition
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                    .AddSwaggerGen(c =>
                    {
                        var security = new Dictionary<string, IEnumerable<string>>
                    {
                        {"Bearer", new string[] { }},
                    };

                        c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                        {
                            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                            Name = "Authorization",
                            In = "header",
                            Type = "apiKey"
                        });
                        c.AddSecurityRequirement(security);
                    });

            return services;
        }
    }

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
          this.provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                  description.GroupName,
                    new Info()
                    {
                        Title = $"Canopy Incident API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                    });
            }
        }
    }
}
