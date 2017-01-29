namespace Easy.Web.Tests.Integration
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Easy.Web.Core.Extensions;
    using Easy.Web.Core.Interfaces;
    using Easy.Web.Serialization.JSONNet;
    using Easy.Web.Serialization.ProtobufNet;
    using Easy.Web.Serialization.XML;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NUnit.Framework;

    public class Context
    {
        [Test]
        public async Task Start()
        {
            const string Endpoint = "http://localhost:1234";
            var server = GetServer(Endpoint);

            var serverTask = Task.Run(() => server.Run());

            var client = new HttpClient { BaseAddress = new Uri(Endpoint) };
            await ReplyTests.Run(client);
            await SerializeTests.Run(client);
            await ContentNegotiationTests.Run(client);
            await BindingQueryStringTests.Run(client);
            await BindingBodyTests.Run(client);
            await ValidationTests.Run(client);
            await RouteDataTests.Run(client);

            server.Dispose();

            await serverTask;
            client.Dispose();
        }

        public IWebHost GetServer(string endpoint)
        {
            return new WebHostBuilder()
                .UseWebListener()
                .UseUrls(endpoint)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class Startup
        {
            // This method gets called by the runtime. Use this method to add services to the container.
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddEasyWeb();
                services.AddSingleton<ISerializer, JSONNetSerializer>();
                services.AddSingleton<ISerializer, XMLSerializer>();
                services.AddSingleton<ISerializer, ProtobufNetSerializer>();
                services.AddSingleton<IDeserializer, JSONNetDeserializer>();
                services.AddSingleton<IDeserializer, XMLDeserializer>();
                services.AddSingleton<IDeserializer, ProtobufNetDeserializer>();
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
            {
                if (env.IsDevelopment())
                {
                    loggerFactory.AddConsole();
                    app.UseDeveloperExceptionPage();
                }

                app.UseEasyWeb(serviceProvider);

                app.Run(async context =>
                {
                    await context.Response.WriteAsync("You will only see this message if no handler handled the request.");
                });
            }
        }
    }
}