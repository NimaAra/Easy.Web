namespace Easy.Web.Tests.Unit.Routing
{
    using Core.Models;
    using Core.Routing;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Http.Authentication;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using NSubstitute;
    using NUnit.Framework;
    using Shouldly;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;

    [TestFixture]
    internal sealed class RequestDispatcherTests
    {
        [Test]
        public async Task When_dispatching_request_to_an_instance_method()
        {
            var handlerType = typeof(SomeHandler);
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(handlerType).Returns(new SomeHandler());

            Func<IServiceProvider, Handler> factory = provider => (Handler)serviceProvider.GetService(handlerType);
            var method = handlerType.GetMethod("InstanceMethod");

            var dispatcher = new RequestDispatcher(handlerType, factory, method);

            var ctx = new DummyContext();
            await dispatcher.DispatchAsync(ctx);
            ctx.TraceIdentifier.ShouldBe("Traced Instance");
        }

        [Test]
        public async Task When_dispatching_request_to_a_static_method()
        {
            var handlerType = typeof(SomeHandler);
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(handlerType).Returns(new SomeHandler());

            Func<IServiceProvider, Handler> factory = provider => (Handler)serviceProvider.GetService(handlerType);
            var method = handlerType.GetMethod("StaticMethod");

            var dispatcher = new RequestDispatcher(handlerType, factory, method);

            var ctx = new DummyContext();
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

        [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
        private sealed class DummyContext : HttpContext
        {
            public override void Abort()
            {
                throw new NotImplementedException();
            }

            public override IFeatureCollection Features { get; }
            public override HttpRequest Request { get; }
            public override HttpResponse Response { get; }
            public override ConnectionInfo Connection { get; }
            public override WebSocketManager WebSockets { get; }
            public override AuthenticationManager Authentication { get; }
            public override ClaimsPrincipal User { get; set; }
            public override IDictionary<object, object> Items { get; set; }
            public override IServiceProvider RequestServices { get; set; }
            public override CancellationToken RequestAborted { get; set; }
            public override string TraceIdentifier { get; set; }
            public override ISession Session { get; set; }
        }
    }
}