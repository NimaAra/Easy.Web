namespace Easy.Web.Tests.Integration.Handlers
{
    using System.Threading.Tasks;
    using Easy.Web.Core.Extensions;
    using Easy.Web.Core.Helpers;
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Routing;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Shouldly;

    internal sealed class RouteDataHandler : Handler
    {
        [Route(HttpMethod.GET, "route-data/noRouteData")]
        public Task NoRouteData(HttpContext context)
        {
            var routeData = context.GetRouteData();

            routeData.ShouldNotBeNull();
            routeData.DataTokens.ShouldBeEmpty();
            routeData.Values.ShouldBeEmpty();

            return context.ReplyAsStatus(HttpStatusCode.Okay);
        }

        [Route(HttpMethod.GET, "route-data/{someController}/{someAction}/{id:int}")]
        public Task WithRouteData(HttpContext context)
        {
            var routeData = context.GetRouteData();

            routeData.ShouldNotBeNull();
            routeData.DataTokens.ShouldBeEmpty();

            routeData.Values.ShouldNotBeEmpty();
            routeData.Values.Count.ShouldBe(3);
            routeData.Values["someController"].ShouldBe("myController");
            routeData.Values["someAction"].ShouldBe("myAction");
            routeData.Values["id"].ShouldBe("1");

            context.GetRouteValue("someController").ShouldBe("myController");
            context.GetRouteValue("someAction").ShouldBe("myAction");
            context.GetRouteValue("id").ShouldBe("1");

            return context.ReplyAsStatus(HttpStatusCode.Okay);
        }
    }
}