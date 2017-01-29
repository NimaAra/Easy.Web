namespace Easy.Web.Sample.Handlers
{
    using System.Threading.Tasks;
    using Core.Models;
    using Core.Helpers;
    using Core.Routing;
    using Core.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public sealed class SampleHandler : Handler
    {
        private readonly ILoggerFactory _loggerFactory;

        public SampleHandler(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory; // Example of dependency injection.
        }

        [Route(HttpMethod.GET, "instance/bye")]
        public Task SayBye(HttpContext context)
        {
            return context.Response.WriteAsync("Bye from an instance handler");
        }

        [Route(HttpMethod.GET, "static/bye")]
        public static Task SayByeStatic(HttpContext context)
        {
            return context.Reply("Bye from a static handler!", MediaTypes.TEXT, HttpStatusCode.Okay);
        }

        [Route(HttpMethod.GET, "bind")]
        public static Task Bind(HttpContext context)
        {
            var dynDic = context.BindToQueryString();

            var id = dynDic["ID"];
            var category = dynDic["category"];

            return context.ReplyAsText($"Id: {id} - Category: {category}");
        }
    }
}