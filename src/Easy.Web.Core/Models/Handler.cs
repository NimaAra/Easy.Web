namespace Easy.Web.Core.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Easy.Web.Core.Routing;

    /// <summary>
    /// Provides an abstraction of a request handler.
    /// </summary>
    public abstract class Handler
    {
        // ReSharper disable once InconsistentNaming
        private static RouteRegistration[] _routeRegistrations;

        /// <summary>
        /// Sets the <c>Easy.Web</c> route registrations for the application.
        /// </summary>
        /// <param name="routesAndDispatchers"></param>
        internal static void SetRegistrations(Dictionary<RouteAttribute, RequestDispatcher> routesAndDispatchers)
        {
            _routeRegistrations = routesAndDispatchers.Select(x => new RouteRegistration(x.Key, x.Value)).ToArray();
        }

        /// <summary>
        /// Gets all the <c>Easy.Web</c> routes registered in the application.
        /// </summary>
        public IEnumerable<RouteRegistration> AllRegistrations => _routeRegistrations;

        /// <summary>
        /// Gets all the <c>Easy.Web</c> routes registered for the instance of <see cref="Handler"/>.
        /// </summary>
        public IEnumerable<RouteRegistration> HandlerRegistrations => Get();

        private IEnumerable<RouteRegistration> Get()
        {
            var type = GetType();
            foreach (var item in _routeRegistrations)
            {
                if (item.HandlingType == type)
                {
                    yield return item;
                }
            }
        }
    }
}