namespace Easy.Web.Tests.Unit.ExtensionsTests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using Shouldly;

    using System.Runtime.CompilerServices;
    using Easy.Web.Core.Extensions;
    using Core.Models;
    using Core.Routing;
    using System.Linq;
    using Easy.Web.Tests.Unit.Handlers;
    using Microsoft.AspNetCore.Http;

    [TestFixture]
    internal sealed class MethodInfoExtensionsTests
    {
        [Test]
        public void When_validating_a_given_method_info_is_a_valid_route_method()
        {
            var allAttributes = typeof(SomeController).RetrieveRoutesAndMethods();

            allAttributes.Length.ShouldBe(15);

            allAttributes.Single(kv => kv.Value.Name == "A_InstanceRouteNoArg")
                .Value.IsValidRouteMethod().ShouldBeFalse();

            allAttributes.Single(kv => kv.Value.Name == "B_StaticRouteNoArg")
                .Value.IsValidRouteMethod().ShouldBeFalse();

            allAttributes.Single(kv => kv.Value.Name == "C_InstanceRouteNoArgAsync")
                .Value.IsValidRouteMethod().ShouldBeFalse();

            allAttributes.Single(kv => kv.Value.Name == "D_StaticRouteNoArgAsync")
                .Value.IsValidRouteMethod().ShouldBeFalse();

            allAttributes.Single(kv => kv.Value.Name == "E_InstanceRouteWithArg")
                .Value.IsValidRouteMethod().ShouldBeFalse();

            allAttributes.Single(kv => kv.Value.Name == "F_StaticRouteWithArg")
                .Value.IsValidRouteMethod().ShouldBeFalse();

            allAttributes.Single(kv => kv.Value.Name == "G_InstanceRouteWithArgAsync")
                .Value.IsValidRouteMethod().ShouldBeTrue();

            allAttributes.Single(kv => kv.Value.Name == "H_StaticRouteWithArgAsync")
                .Value.IsValidRouteMethod().ShouldBeTrue();

            var doubleRoutes = allAttributes.Where(kv => kv.Value.Name == "I_InstanceWithMultipleRoutes").ToArray();
            doubleRoutes[0].Value.IsValidRouteMethod().ShouldBeTrue();
            doubleRoutes[1].Value.IsValidRouteMethod().ShouldBeTrue();

            allAttributes.Single(kv => kv.Value.Name == "J_InstanceGenericRouteNoArgAsync")
                .Value.IsValidRouteMethod().ShouldBeFalse();

            allAttributes.Single(kv => kv.Value.Name == "K_InstanceRouteWithInvalidArgAsync")
                .Value.IsValidRouteMethod().ShouldBeFalse();

            allAttributes.Single(kv => kv.Value.Name == "L_InstanceRouteWithTooManyArgAsync")
                .Value.IsValidRouteMethod().ShouldBeFalse();

            allAttributes.Single(kv => kv.Value.Name == "M_InstanceRouteWithTooManyArgAsync")
                .Value.IsValidRouteMethod().ShouldBeFalse();

            allAttributes.Single(kv => kv.Value.Name == "N_StaticRouteWithArgAsync")
                .Value.IsValidRouteMethod().ShouldBeFalse();
        }

        [Test]
        public void When_creating_delegate_from_instance_route_method()
        {
            var instance = new ParentHandler();
            var method = instance.GetType().GetMethod("OnGet");
            method.ShouldNotBeNull();

            var funcAsync = method.CreateDelegateForInstanceMethod(instance.GetType());
            funcAsync.ShouldNotBeNull();
            funcAsync.Target.ShouldNotBeNull();
            funcAsync.Target.GetType().ShouldNotBeNull();

            funcAsync.Method.ShouldNotBeNull();
            funcAsync.Method.ReturnType.ShouldBe(typeof(Task));

            var funcParams = funcAsync.Method.GetParameters();
            funcParams.ShouldNotBeNull();
            funcParams.Length.ShouldBe(3);
            funcParams[0].ParameterType.ShouldBe(typeof(Closure));
            funcParams[1].ParameterType.ShouldBe(typeof(Handler));
            funcParams[2].ParameterType.ShouldBe(typeof(HttpContext));
        }

        [Test]
        public void When_creating_delegate_from_static_route_method()
        {
            var method = typeof(ParentHandler).GetMethod("OnDelete");
            method.ShouldNotBeNull();

            var funcAsync = method.CreateDelegateForStaticMethod(typeof(ParentHandler));
            funcAsync.ShouldNotBeNull();
            funcAsync.Target.ShouldNotBeNull();
            funcAsync.Method.ShouldNotBeNull();
            funcAsync.Method.ReturnType.ShouldBe(typeof(Task));

            var funcParams = funcAsync.Method.GetParameters();
            funcParams.ShouldNotBeNull();
            funcParams.Length.ShouldBe(2);
            funcParams[0].ParameterType.ShouldBe(typeof(Closure));
            funcParams[1].ParameterType.ShouldBe(typeof(HttpContext));
        }

        [Test]
        public void When_creating_delegate_from_static_void_route_method()
        {
            var method = typeof(SomeController).GetMethod("H_StaticRouteWithArgAsync");
            method.ShouldNotBeNull();

            var funcAsync = method.CreateDelegateForStaticMethod(typeof(SomeController));
            funcAsync.ShouldNotBeNull();
            funcAsync.Target.ShouldNotBeNull();
            funcAsync.Method.ShouldNotBeNull();
            funcAsync.Method.ReturnType.ShouldBe(typeof(Task));

            var funcParams = funcAsync.Method.GetParameters();
            funcParams.ShouldNotBeNull();
            funcParams.Length.ShouldBe(2);
            funcParams[0].ParameterType.ShouldBe(typeof(Closure));
            funcParams[1].ParameterType.ShouldBe(typeof(HttpContext));
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        [SuppressMessage("ReSharper", "UnusedTypeParameter")]
        private class SomeController
        {
            [Route(HttpMethod.GET, "1")]
            public void A_InstanceRouteNoArg() { }

            [Route(HttpMethod.GET, "2")]
            public void B_StaticRouteNoArg() { }

            [Route(HttpMethod.POST, "3")]
            public Task C_InstanceRouteNoArgAsync() => Task.FromResult(0);

            [Route(HttpMethod.POST, "4")]
            public static Task D_StaticRouteNoArgAsync() => Task.FromResult(0);

            [Route(HttpMethod.GET, "5")]
            public void E_InstanceRouteWithArg(HttpContext context) { }

            [Route(HttpMethod.POST, "6")]
            public static void F_StaticRouteWithArg(HttpContext context) { }

            [Route(HttpMethod.PUT, "7")]
            public Task G_InstanceRouteWithArgAsync(HttpContext context) => Task.FromResult(0);

            [Route(HttpMethod.DELETE, "8")]
            public static Task H_StaticRouteWithArgAsync(HttpContext context) => Task.FromResult(0);

            [Route(HttpMethod.CONNECT, "9")]
            [Route(HttpMethod.PATCH, "10")]
            public Task I_InstanceWithMultipleRoutes(HttpContext context) => Task.FromResult(0);

            [Route(HttpMethod.GET, "11")]
            public Task<string> J_InstanceGenericRouteNoArgAsync() => Task.FromResult("foo");

            [Route(HttpMethod.GET, "12")]
            public void K_InstanceRouteWithInvalidArgAsync(string foo) { }

            [Route(HttpMethod.GET, "13")]
            public void L_InstanceRouteWithTooManyArgAsync(HttpContext context, string foo) { }

            [Route(HttpMethod.GET, "14")]
            public void M_InstanceRouteWithTooManyArgAsync(out HttpContext context) { context = null; }

            [Route(HttpMethod.DELETE, "15")]
            public static Task N_StaticRouteWithArgAsync<T>(HttpContext context) => Task.FromResult(0);
        }
    }
}