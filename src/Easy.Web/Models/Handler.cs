namespace Easy.Web.Core.Models
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Easy.Web.Core.Routing;
    using System.ComponentModel.DataAnnotations;

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
        [DebuggerStepThrough]
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

        /// <summary>
        /// Attempts to validate the <paramref name="model"/> by using <see cref="Validator"/>.
        /// </summary>
        public bool TryValidateModel<T>(T model, out List<ValidationResult> result)
        {
            result = new List<ValidationResult>();
            return Validator.TryValidateObject(model, new ValidationContext(model), result, true);
        }

        [DebuggerStepThrough]
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