namespace Easy.Web.Sample.Handlers
{
    using System.Threading.Tasks;
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Routing;
    using Microsoft.AspNetCore.Http;

    public sealed class HandlerTwo : Handler
    {
        [Route(HttpMethod.GET, "instance/bye")]
        public Task SayByte(HttpContext context)
        {
            return context.Response.WriteAsync("Bye from Handler");
        }

        [Route(HttpMethod.GET, "static/bye")]
        public static Task SayByeStatic(HttpContext context)
        {
            return context.Response.WriteAsync("Bye from Static Handler");
        }
    }
}