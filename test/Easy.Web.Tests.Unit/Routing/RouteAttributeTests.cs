namespace Easy.Web.Tests.Unit.Routing
{
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Routing;
    using System;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    internal sealed class RouteAttributeTests
    {
        [Test]
        public void When_creating_a_route_attribute()
        {
            var attr = new RouteAttribute(HttpMethod.DELETE, "foo/{controller}");
            attr.Method.ShouldBe(HttpMethod.DELETE);
            attr.Pattern.ShouldBe("foo/{controller}");
            attr.ToString().ShouldBe("DELETE | foo/{controller}");
        }

        [Test]
        public void When_creating_a_route_attribute_with_invalid_path()
        {
            var e = Should.Throw<ArgumentException>(() => new RouteAttribute(HttpMethod.GET, null));
            e.Message.ShouldBe("Route pattern must not be null, empty or whitespace.\r\nParameter name: routePattern");
            e.ParamName.ShouldBe("routePattern");

            e = Should.Throw<ArgumentException>(() => new RouteAttribute(HttpMethod.GET, string.Empty));
            e.Message.ShouldBe("Route pattern must not be null, empty or whitespace.\r\nParameter name: routePattern");
            e.ParamName.ShouldBe("routePattern");

            e = Should.Throw<ArgumentException>(() => new RouteAttribute(HttpMethod.GET, " "));
            e.Message.ShouldBe("Route pattern must not be null, empty or whitespace.\r\nParameter name: routePattern");
            e.ParamName.ShouldBe("routePattern");
        }

        [Test]
        public void When_comparing_equal_routes()
        {
            var routeOne = new RouteAttribute(HttpMethod.GET, "foo");
            var routeTwo = new RouteAttribute(HttpMethod.GET, "foo");

            routeOne.Equals(routeTwo).ShouldBeTrue();
        }

        [Test]
        public void When_comparing_non_equal_routes()
        {
            var routeOne = new RouteAttribute(HttpMethod.GET, "foo");
            var routeTwo = new RouteAttribute(HttpMethod.GET, "bar");

            routeOne.Equals(routeTwo).ShouldBeFalse();

            routeOne = new RouteAttribute(HttpMethod.GET, "foo");
            routeTwo = new RouteAttribute(HttpMethod.DELETE, "foo");

            routeOne.Equals(routeTwo).ShouldBeFalse();
        }
    }
}