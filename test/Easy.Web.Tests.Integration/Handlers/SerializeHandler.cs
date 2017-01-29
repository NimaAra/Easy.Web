namespace Easy.Web.Tests.Integration.Handlers
{
    using System.Threading.Tasks;
    using Easy.Web.Core.Extensions;
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Routing;
    using Easy.Web.Tests.Integration.Models;
    using Microsoft.AspNetCore.Http;

    internal sealed class SerializeHandler : Handler
    {
        [Route(HttpMethod.GET, "serialize/json")]
        public Task SerializeToJSON(HttpContext context)
        {
            var model = new SampleModel { Id = 123, Category = "Some category" };
            return context.SerializeAsJSON(model);
        }

        [Route(HttpMethod.GET, "serialize/xml")]
        public Task SerializeToXML(HttpContext context)
        {
            var model = new SampleModel { Id = 123, Category = "Some category" };
            return context.SerializeAsXML(model);
        }

        [Route(HttpMethod.GET, "serialize/binary")]
        public Task SerializeToBinary(HttpContext context)
        {
            var model = new SampleModel { Id = 123, Category = "Some category" };
            return context.SerializeAsBinary(model);
        }
    }
}