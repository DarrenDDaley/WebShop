using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cart.API.Managers;
using Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace Cart.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                 .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterType<CatalogManager>().As<ICatalogManager>().SingleInstance();
            builder.RegisterType<CartManager>().As<ICartManager>().SingleInstance();

            IEndpointInstance endpoint = null;
            builder.Register(c => endpoint)
                .As<IEndpointInstance>()
                .SingleInstance();


            this.ApplicationContainer = builder.Build();

            var endpointConfiguration = new EndpointConfiguration("Cart.API");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.SendFailedMessagesTo("error");

            var routing = transport.Routing();
            routing.RouteToEndpoint(
                assembly: typeof(PlaceOrder).Assembly,
                destination: "Orders.API");

            endpointConfiguration.UseContainer<AutofacBuilder>(
             customizations: customizations =>
              {
                 customizations.ExistingLifetimeScope(ApplicationContainer);
              });

            endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env, 
                              IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
