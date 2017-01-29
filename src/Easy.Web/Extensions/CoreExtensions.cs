namespace Easy.Web.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using Easy.Web.Core.Exceptions;
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Routing;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Provides a set of helper methods
    /// </summary>
    public static class CoreExtensions
    {
        private static readonly List<Type> Handlers = new List<Type>();
        private static readonly Dictionary<RouteAttribute, RequestDispatcher> RouteToRequestDispatchers =
            new Dictionary<RouteAttribute, RequestDispatcher>();

        /// <summary>
        /// Adds <c>EasyWeb</c> as a service.
        /// <remarks>
        /// This methods enables the framework to detect any instance of <see cref="Handler"/> in 
        /// the application which need to be registered for request handling.
        /// </remarks>
        /// </summary>
        public static void AddEasyWeb(this IServiceCollection services)
        {
            foreach (var type in GetHandlerTypes())
            {
                services.AddScoped(type);
                Handlers.Add(type);
            }

            EnsureRegistrationIsSuccess();
            services.AddRouting();
        }

        /// <summary>
        /// Instructs the application pipeline to use the <c>EasyWeb</c> framework for request routing.
        /// </summary>
        public static void UseEasyWeb(this IApplicationBuilder appBuilder, IServiceProvider serviceProvider)
        {
            EnsureRegistrationIsSuccess();

            var routeBuilder = new RouteBuilder(appBuilder);

            foreach (var type in Handlers)
            {
                try
                {
                    // ReSharper disable once UnusedVariable
                    var ignored = serviceProvider.GetService(type);
                }
                catch (Exception e)
                {
                    throw new HandlerCreationException($"Could not create an instance of the `{type}`, have you registered all it's dependencies? Error:{Environment.NewLine}{e}");
                }

                var routesAndMethods = type.RetrieveRoutesAndMethods();

                if (routesAndMethods.Length == 0)
                {
                    throw new HandlerCreationException(
                        $"Could not find any method marked with `{nameof(RouteAttribute)}` in handler: `{type.Name}`, did you forget to add any?");
                }

                foreach (var item in routesAndMethods)
                {
                    var route = item.Key;
                    var method = item.Value;

                    if (!method.IsValidRouteMethod())
                    {
                        throw new RouteRegistrationException($"Invalid method: `{method.Name}` flagged with the `{nameof(RouteAttribute)}` defined in handler: `{type.Name}`, a valid method should be a `public`, `static` or `instance`, `non-generic` method accepting `one parameter` of type `{typeof(HttpContext)}` and returning a `{typeof(Task)}`.");
                    }

                    RequestDispatcher existingDispatcher;
                    if (RouteToRequestDispatchers.TryGetValue(route, out existingDispatcher))
                    {
                        throw new RouteRegistrationException($"Duplicate route detected when registering route: `{route}` on method: `{method.Name}` in handler: `{type.Name}`, one has already been defined on method: `{existingDispatcher.HandlingMethod.Name}` in handler: `{existingDispatcher.HandlingType.Name}`.");
                    }

                    var dispatcher = new RequestDispatcher(type, method);
                    RegisterRouteAndDispatcher(routeBuilder, route, dispatcher);
                }
            }
            
            Handler.SetRegistrations(RouteToRequestDispatchers);

            var router = routeBuilder.Build();
            appBuilder.UseRouter(router);
        }

        private static IEnumerable<Type> GetHandlerTypes()
        {
#if NET_STANDARD
            
            foreach (var type in Assembly.GetEntryAssembly().GetTypes())
            {
                var typeInfo = type.GetTypeInfo();
                if (!typeInfo.IsSubclassOf(typeof(Handler))) { continue; }
                yield return type;
            }
#else
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsSubclassOf(typeof(Handler))) { continue; }
                    yield return type;
                }
            }
#endif
        }

        private static void RegisterRouteAndDispatcher(IRouteBuilder builder, RouteAttribute route, RequestDispatcher dispatcher)
        {
            builder.MapVerb(route.Method.ToString(), route.Pattern, dispatcher.DispatchAsync);
            RouteToRequestDispatchers.Add(route, dispatcher);
        }

        private static void EnsureRegistrationIsSuccess()
        {
            if (Handlers.Count == 0)
            {
                throw new HandlerCreationException($"Unable to detect any handlers. Have you declared your implementation(s) of `{nameof(Handler)}` and calling the `{nameof(AddEasyWeb)}()` on: `{nameof(IServiceCollection)}` in the `ConfigureServices` method in the `Startup` or the equivalent class?");
            }
        }
    }
}