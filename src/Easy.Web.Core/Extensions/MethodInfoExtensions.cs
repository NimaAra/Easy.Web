namespace Easy.Web.Core.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Routing;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Provides a set of helper methods for working with <see cref="MethodInfo"/>.
    /// </summary>
    internal static class MethodInfoExtensions
    {
        /// <summary>
        /// Check if the given <paramref name="method"/> is a valid method for a <see cref="RouteAttribute"/> to invoke.
        /// </summary>
        [DebuggerStepThrough]
        internal static bool IsValidRouteMethod(this MethodInfo method)
        {
            if (method.ReturnType == typeof(Task))
            {
                if (method.ContainsGenericParameters) { return false; }

                var methodParams = method.GetParameters();

                if (methodParams.Length == 1 
                    && methodParams[0].ParameterType == typeof(HttpContext) 
                    && !methodParams[0].IsOut)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a <see cref="Func{Handler, RequestContext, Task}"/> from the given instance <paramref name="method"/>.
        /// </summary>
        // ReSharper disable once UnusedParameter.Global Used so that compiler can deduce T
        [DebuggerStepThrough]
        internal static Func<Handler, HttpContext, Task> CreateDelegateForInstanceMethod(this MethodInfo method, Type handlerType)
        {
            var instanceParam = Expression.Parameter(typeof(Handler), "handler");
            var argParam = Expression.Parameter(typeof(HttpContext), "requestCotnext");
            var body = Expression.Call(
                Expression.Convert(instanceParam, handlerType),
                method,
                argParam);

            return Expression.Lambda<Func<Handler, HttpContext, Task>>(body, instanceParam, argParam).Compile();
        }

        /// <summary>
        /// Creates a <see cref="Func{RequestContext, Task}"/> from the given static <paramref name="method"/>.
        /// </summary>
        [DebuggerStepThrough]
        internal static Func<HttpContext, Task> CreateDelegateForStaticMethod(this MethodInfo method, Type handlerType)
        {
            var argParam = Expression.Parameter(typeof(HttpContext), "requestCotnext");
            var mCallStatic = Expression.Call(handlerType, method.Name, null, argParam);
            return Expression.Lambda<Func<HttpContext, Task>>(mCallStatic, argParam).Compile();
        }
    }
}