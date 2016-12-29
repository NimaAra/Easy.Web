namespace Easy.Web.Core.Exceptions
{
    using System;
    using Easy.Web.Core.Models;

    /// <summary>
    /// Provides an abstraction for any exceptions thrown during the creation of an instance of <see cref="Handler"/>.
    /// </summary>
    public sealed class HandlerCreationException : Exception
    {
        /// <summary>
        /// Creates an instance of the <see cref="HandlerCreationException"/>.
        /// </summary>
        public HandlerCreationException() { }

        /// <summary>
        /// Creates an instance of the <see cref="HandlerCreationException"/>.
        /// </summary>
        /// <param name="message">The message for the <see cref="Exception"/></param>
        public HandlerCreationException(string message) : base(message) { }

        /// <summary>
        /// Creates an instance of the <see cref="HandlerCreationException"/>.
        /// </summary>
        /// <param name="message">The message for the <see cref="Exception"/></param>
        /// <param name="innerException">The inner exception</param>
        public HandlerCreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}