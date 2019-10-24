using Autofac;
using Autofac.Extensions.DependencyInjection;
using CanopyManage.Application.Compositions;
using CanopyManage.Common.Logger;
using CanopyManage.Infrastructure.Compositions;
using CanopyManage.WebService.Compositions;
using CanopyManage.WebService.Infrastructure.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CanopyManage.WebService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
                    .AddMediator()
                    .AddRepository();

            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
            .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

            /*
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://login.microsoftonline.com/dc1473ea-4cea-43c3-ad60-4aa5a747f8d7/";
                options.RequireHttpsMetadata = false;
                options.ClientId = "8ccfbbec-a979-4aaf-bdf4-627bd39b39c6";
                options.ClientSecret = "i=gx73/TonYZ:yI4U9GUgJZIPSu/zf@7";
                options.ResponseType = "code id_token";
                options.SaveTokens = true;
                options.gra
                
                options.GetClaimsFromUserInfoEndpoint = true;
            // options.Scope.Add();
            options.Scope.Add("email");
                options.Scope.Add("offline_access");
                //options.Events = AddTenantToOIDCEvent(appConfig.Value.ApplicationId);
            });
              */
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new FluentValidationModule());

            return new AutofacServiceProvider(builder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            // Enable middle-ware to serve generated Swagger as a JSON endpoint.
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
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            //var eventBus = app.ApplicationServices.GetRequiredService<IEventBusPublisher>();

            //eventBus.SubscribeAsync<UserCheckoutIntegrationEvent,
            //   IIntegrationEventHandler<UserCheckoutIntegrationEvent>>();
        }
    }
}
