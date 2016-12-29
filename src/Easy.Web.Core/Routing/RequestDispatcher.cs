namespace Easy.Web.Core.Routing
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Easy.Web.Core.Extensions;
    using Easy.Web.Core.Models;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Provides an abstraction for dispatching a <see cref="HttpContext"/> to a <see cref="Handler"/>.
    /// </summary>
    internal sealed class RequestDispatcher
    {
        private readonly bool _isStatic;
        private readonly Func<HttpContext, Task> _staticHandler;
        private readonly Func<Handler, HttpContext, Task> _instanceHandler;

        /// <summary>
        /// Creates an instance of the <see cref="RequestDispatcher"/>.
        /// </summary>
        /// <param name="handlerType">The type of handler.</param>
        /// <param name="method">The <see cref="MethodInfo"/> marked for handling the request.</param>
        internal RequestDispatcher(Type handlerType, MethodInfo method)
        {
            HandlingType = handlerType;
            HandlingMethod = method;

            if (HandlingMethod.IsStatic)
            {
                _isStatic = true;
                _staticHandler = HandlingMethod.CreateDelegateForStaticMethod(HandlingType);
            }
            else
            {
                _instanceHandler = HandlingMethod.CreateDelegateForInstanceMethod(HandlingType);
            }
        }

        /// <summary>
        /// Gets the type of the handler.
        /// </summary>
        public Type HandlingType { get; }

        /// <summary>
        /// Gets the method information for handling the request.
        /// </summary>
        public MethodInfo HandlingMethod { get; }

        /// <summary>
        /// Dispatches the <paramref name="context"/> to the handler.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal async Task DispatchAsync(HttpContext context)
        {
            if (_isStatic)
            {
                await _staticHandler(context);
            }
            else
            {
                var instance = context.RequestServices.GetService(HandlingType) as Handler;
                await _instanceHandler(instance, context);

                // ReSharper disable once SuspiciousTypeConversion.Global
                var disposableInstance = instance as IDisposable;
                disposableInstance?.Dispose();
            }
        }
    }
}