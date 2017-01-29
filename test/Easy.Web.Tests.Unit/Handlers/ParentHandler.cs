namespace Easy.Web.Tests.Unit.Handlers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Routing;
    using Microsoft.AspNetCore.Http;

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public class ParentHandler : Handler
    {
        [Route(HttpMethod.GET, "/foo")]
        public Task OnGet(HttpContext context) => Task.FromResult(0);

        [Route(HttpMethod.POST, "/bar/{controller}")]
        public Task OnPost(HttpContext context) => Task.FromResult(0);

        [Route(HttpMethod.DELETE, "/bool/")]
        public static Task OnDelete(HttpContext context) => Task.FromResult(0);
    }
}