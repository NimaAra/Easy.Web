namespace Easy.Web.Tests.Unit.TestModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Core.Models;
    using Core.Routing;

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public class ParentController : Handler
    {
        [Route(HttpMethod.GET, "/foo")]
        public Task OnGet(HttpContext context) => Task.FromResult(0);

        [Route(HttpMethod.POST, "/bar/{controller}")]
        public Task OnPost(HttpContext context) => Task.FromResult(0);

        [Route(HttpMethod.DELETE, "/bool/")]
        public static Task OnDelete(HttpContext context) => Task.FromResult(0);
    }
}