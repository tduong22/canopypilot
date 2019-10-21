using Autofac;
using Autofac.Extensions.DependencyInjection;
using CanopyManage.Application.Compositions;
using CanopyManage.Common.EventBus.Abstractions;
using CanopyManage.Common.Logger;
using CanopyManage.IncidentService.Compositions;
using CanopyManage.IncidentService.Infrastructure.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
                    .AddMediator();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new FluentValidationModule());

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

            //eventBus.SubscribeAsync<UserCheckoutIntegrationEvent,
            //   IIntegrationEventHandler<UserCheckoutIntegrationEvent>>();
        }
    }
}
