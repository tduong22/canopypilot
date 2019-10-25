using Autofac;
using Autofac.Extensions.DependencyInjection;
using CanopyManage.Application.Compositions;
using CanopyManage.Application.IntegrationEvents.Events;
using CanopyManage.Common.EventBus.Abstractions;
using CanopyManage.Common.Logger;
using CanopyManage.IncidentService.Compositions;
using CanopyManage.IncidentService.Infrastructure.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CanopyManage.IncidentService
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                   .AddControllersAsServices();
            services.AddApiVersioning();
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            services.RegisterLogger(Configuration["Logging:InstrumentationKey"])
                    .AddSwagger()
                    .AddEventBusPublisher(Configuration["ServiceBus:ConnectionString"], Environment.EnvironmentName)
                    .AddEventBusSubscriber(Configuration["ServiceBus:ConnectionString"], Configuration["ServiceBus:SubscriptionClientName"])
                    .AddMediator()
                    .AddExternalServices();

            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
          .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new EventHandlingModule())
                .RegisterModule(new FluentValidationModule());

            return new AutofacServiceProvider(builder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBusSubscriber>();
            var filterProperties = new Dictionary<string, object>
            {
                { "AlertType", "ServiceNow" }
            };
            eventBus.SubscribeAsync<IncidentSubmittedIntegrationEvent,
               IIntegrationEventHandler<IncidentSubmittedIntegrationEvent>>(filterProperties);
        }
    }
}
