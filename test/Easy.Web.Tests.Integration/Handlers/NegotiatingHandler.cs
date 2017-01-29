namespace Easy.Web.Tests.Integration.Handlers
{
    using System.Threading.Tasks;
    using Easy.Web.Core.Extensions;
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Routing;
    using Easy.Web.Tests.Integration.Models;
    using Microsoft.AspNetCore.Http;

    internal sealed class NegotiatingHandler : Handler
    {
        [Route(HttpMethod.GET, "negotiate")]
        public Task Negotiate(HttpContext context)
        {
            var model = new SampleModel { Id = 123, Category = "Some category" };
            return context.Negotiate(model);
        }
    }
}