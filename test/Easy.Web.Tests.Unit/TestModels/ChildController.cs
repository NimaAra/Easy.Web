namespace Easy.Web.Tests.Unit.TestModels
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Core.Models;
    using Core.Routing;

    public sealed class ChildController : ParentController
    {
        [Route(HttpMethod.PUT, "/baz")]
        public Task OnPut(HttpContext context) => Task.FromResult(0);
    }
}