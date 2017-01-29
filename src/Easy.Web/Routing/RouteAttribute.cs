namespace Easy.Web.Core.Routing
{
    using System;
    using System.Diagnostics;
    using Easy.Web.Core.Models;

    /// <summary>
    /// An abstraction for representing a route.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class RouteAttribute : Attribute, IEquatable<RouteAttribute>
    {
        /// <summary>
        /// Creates an instance of the <see cref="RouteAttribute"/>.
        /// </summary>
        /// <param name="method">The <c>HTTP</c> method which this route matches.</param>
        /// <param name="routePattern">The pattern which this route matches.</param>
        [DebuggerStepThrough]
        public RouteAttribute(HttpMethod method, string routePattern)
        {
            if (string.IsNullOrWhiteSpace(routePattern))
            {
                throw new ArgumentException("Route pattern must not be null, empty or whitespace.", nameof(routePattern));
            }

            Method = method;
            Pattern = routePattern;
        }

        /// <summary>
        /// Gets the method which this route matches.
        /// </summary>
        public HttpMethod Method { get; }

        /// <summary>
        /// Gets the pattern which this route matches.
        /// </summary>
        public string Pattern { get; }

        /// <summary>
        /// Provides textual representation of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Concat(Method.ToString(), " | ", Pattern);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified <paramref name="other"/>. 
        /// </summary>
        public bool Equals(RouteAttribute other)
        {
            if (other == null) { return false; }
            return ReferenceEquals(this, other) || GetHashCode().Equals(other.GetHashCode());
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return Method.GetHashCode() + Pattern.GetHashCode();
        }
    }
}
