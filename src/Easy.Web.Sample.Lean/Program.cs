namespace Easy.Web.Sample.Lean
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Helpers;
    using Easy.Web.Core.Routing;
    using Easy.Web.Core.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables()
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(l => l.AddConsole(config.GetSection("Logging")))
                .ConfigureServices(services =>
                {
                    services.AddEasyWeb();
                })
                .Configure(app =>
                {
                    app.UseEasyWeb(app.ApplicationServices);
                })
                .Build();

            host.Run();
        }

        public sealed class DefaultHandler : Handler
        {
            [Route(HttpMethod.GET, "api")]
            public Task DefaultAction(HttpContext context)
            {
                return context.ReplyAsText("A simple message from a simple action!", HttpStatusCode.Okay);
            }
        }
    }
}
