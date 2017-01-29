namespace Easy.Web.Tests.Unit.Routing
{
    using Core.Models;
    using Core.Routing;
    using System;
    using System.Threading.Tasks;
    using Easy.Web.Tests.Unit.Models;
    using NSubstitute;
    using NUnit.Framework;
    using Shouldly;
    using Microsoft.AspNetCore.Http;

    [TestFixture]
    internal sealed class RequestDispatcherTests
    {
        [Test]
        public async Task When_dispatching_request_to_an_instance_method()
        {
            var handlerType = typeof(SomeHandler);
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(handlerType).Returns(new SomeHandler());

            var method = handlerType.GetMethod("InstanceMethod");

            var dispatcher = new RequestDispatcher(handlerType, method);

            var ctx = new DummyContext(serviceProvider);
            await dispatcher.DispatchAsync(ctx);
            ctx.TraceIdentifier.ShouldBe("Traced Instance");
        }

        [Test]
        public async Task When_dispatching_request_to_a_static_method()
        {
            var handlerType = typeof(SomeHandler);
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(handlerType).Returns(new SomeHandler());

            var method = handlerType.GetMethod("StaticMethod");

            var dispatcher = new RequestDispatcher(handlerType, method);

            var ctx = new DummyContext(serviceProvider);
            await dispatcher.DispatchAsync(ctx);
            ctx.TraceIdentifier.ShouldBe("Traced Static");
        }

        private sealed class SomeHandler : Handler
        {
            [Route(HttpMethod.GET, "/api")]
            public Task InstanceMethod(HttpContext context)
            {
                context.TraceIdentifier = "Traced Instance";
                return Task.FromResult(0);
            }

            [Route(HttpMethod.POST, "/api")]
            public static Task StaticMethod(HttpContext context)
            {
                context.TraceIdentifier = "Traced Static";
                return Task.FromResult(0);
            }
        }
    }
}