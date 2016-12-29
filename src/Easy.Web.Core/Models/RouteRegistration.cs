namespace Easy.Web.Core.Models
{
    using System;
    using System.Reflection;
    using Easy.Web.Core.Routing;

    /// <summary>
    /// Provides an abstraction for representing a route registration.
    /// </summary>
    public sealed class RouteRegistration
    {
        private readonly RequestDispatcher _dispatcher;

        /// <summary>
        /// Creates an instance of the <see cref="RouteRegistration"/>.
        /// </summary>
        internal RouteRegistration(RouteAttribute route, RequestDispatcher dispatcher)
        {
            Route = route;
            _dispatcher = dispatcher;
        }
        
        /// <summary>
        /// Gets the route.
        /// </summary>
        public RouteAttribute Route { get; }

        /// <summary>
        /// Gets the type of the handler.
        /// </summary>
        public Type HandlingType => _dispatcher.HandlingType;

        /// <summary>
        /// Gets the method information for handling the request.
        /// </summary>
        public MethodInfo HandlingMethod => _dispatcher.HandlingMethod;

        /// <summary>
        /// Gets the textual representation of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Route [Method: {Route.Method.ToString()} | Pattern: {Route.Pattern}] - Handler [Type: {HandlingType.Name} | Method: {HandlingMethod.Name}]";
        }
    }
}