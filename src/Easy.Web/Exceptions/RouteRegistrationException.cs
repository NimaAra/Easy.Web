namespace Easy.Web.Core.Exceptions
{
    using System;

    /// <summary>
    /// Provides an abstraction for any exceptions thrown during route registration.
    /// </summary>
    public sealed class RouteRegistrationException : Exception
    {
        /// <summary>
        /// Creates an instance of the <see cref="RouteRegistrationException"/>.
        /// </summary>
        public RouteRegistrationException() { }

        /// <summary>
        /// Creates an instance of the <see cref="RouteRegistrationException"/>.
        /// </summary>
        /// <param name="message">The message for the <see cref="Exception"/></param>
        public RouteRegistrationException(string message) : base(message) { }

        /// <summary>
        /// Creates an instance of the <see cref="RouteRegistrationException"/>.
        /// </summary>
        /// <param name="message">The message for the <see cref="Exception"/></param>
        /// <param name="innerException">The inner exception</param>
        public RouteRegistrationException(string message, Exception innerException) : base(message, innerException) { }
    }
}