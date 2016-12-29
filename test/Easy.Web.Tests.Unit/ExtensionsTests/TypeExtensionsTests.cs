namespace Easy.Web.Tests.Unit.ExtensionsTests
{
    using Easy.Web.Core.Routing;
    using Easy.Web.Core.Extensions;
    using HttpMethod = Core.Models.HttpMethod;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    internal sealed class TypeExtensionsTests
    {
        [Test]
        public void When_retriving_route_attributes_on_every_public_method()
        {
            var allAttributes = typeof(SomeClass).RetrieveRoutesAndMethods();

            allAttributes.Length.ShouldBe(13);

            allAttributes.ShouldNotContain(kv => kv.Value.Name == "A_InstanceNoAttribute");
            allAttributes.ShouldNotContain(kv => kv.Value.Name == "B_StaticNoAttribute");

            allAttributes.ShouldNotContain(kv => kv.Value.Name == "M_InstancePrivateAsync");
            allAttributes.ShouldNotContain(kv => kv.Value.Name == "N_InstancePrivateAsync");
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private sealed class SomeClass
        {
            public void A_InstanceNoAttribute() { }
            public static void B_StaticNoAttribute() { }

            [Route(HttpMethod.GET, "1")]
            public void C_InstanceRouteNoArg() { }

            [Route(HttpMethod.GET, "2")]
            public void D_StaticRouteNoArg() { }

            [Route(HttpMethod.POST, "3")]
            public Task E_InstanceRouteNoArgAsync() => Task.FromResult(0);

            [Route(HttpMethod.POST, "4")]
            public static Task F_StaticRouteNoArgAsync() => Task.FromResult(0);

            [Route(HttpMethod.GET, "5")]
            public void G_InstanceRouteWithArg(HttpContext context) { }

            [Route(HttpMethod.POST, "6")]
            public static void H_StaticRouteWithArg(HttpContext context) { }

            [Route(HttpMethod.PUT, "7")]
            public Task I_InstanceRouteWithArgAsync(HttpContext context) => Task.FromResult(0);

            [Route(HttpMethod.DELETE, "8")]
            public static Task J_StaticRouteWithArgAsync(HttpContext context) => Task.FromResult(0);

            [Route(HttpMethod.CONNECT, "9")]
            [Route(HttpMethod.PATCH, "10")]
            public Task K_InstanceWithMultipleRoutes(HttpContext context) => Task.FromResult(0);

            [Route(HttpMethod.GET, "11")]
            public Task<string> L_InstanceGenericRouteNoArgAsync() => Task.FromResult("foo");

            [Route(HttpMethod.GET, "12")]
            private Task<string> M_InstancePrivateAsync() => Task.FromResult("foo");

            [Route(HttpMethod.GET, "13")]
            internal Task<string> N_InstancePrivateAsync() => Task.FromResult("foo");

            [Route(HttpMethod.GET, "14")]
            public void O_InstanceRouteWithInvalidArgAsync(string foo) { }

            [Route(HttpMethod.GET, "15")]
            public void P_InstanceRouteWithTooManyArgAsync(HttpContext context, string foo) { }
        }
    }
}