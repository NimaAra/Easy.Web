namespace Easy.Web.Sample.Handlers
{
    using System.Threading.Tasks;
    using Easy.Web.Core.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Core.Routing;
    using Microsoft.Extensions.Logging;

    public sealed class HandlerOne : Handler
    {
        public HandlerOne(ILoggerFactory loggerFactory)
        {
            
        }

        [Route(HttpMethod.GET, "instance/hi")]
        public Task SayHi(HttpContext context)
        {
            
            return context.Response.WriteAsync("Hello from Handler");
        }
    }
}