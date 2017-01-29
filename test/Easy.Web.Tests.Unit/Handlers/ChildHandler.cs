namespace Easy.Web.Tests.Unit.Handlers
{
    using System.Threading.Tasks;
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Routing;
    using Microsoft.AspNetCore.Http;

    public sealed class ChildHandler : ParentHandler
    {
        [Route(HttpMethod.PUT, "/baz")]
        public Task OnPut(HttpContext context) => Task.FromResult(0);
    }
}