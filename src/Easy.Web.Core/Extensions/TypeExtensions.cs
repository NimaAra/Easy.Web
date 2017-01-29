// ReSharper disable LoopCanBeConvertedToQuery
namespace Easy.Web.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using Easy.Web.Core.Helpers;
    using Easy.Web.Core.Routing;

    /// <summary>
    /// Provides a set of helper methods for working with <see cref="Type"/>.
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Retrieves all of the <see cref="RouteAttribute"/> and their corresponding <see cref="MethodInfo"/> 
        /// from the given <paramref name="type"/>.
        /// </summary>
        [DebuggerStepThrough]
        internal static KeyValuePair<RouteAttribute, MethodInfo>[] RetrieveRoutesAndMethods(this Type type)
        {
            var mehotdsAndTheirAttributes = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Select(m => new { MethodInfo = m, Attributes = m.GetCustomAttributes<RouteAttribute>(false).ToArray() })
                .Where(x => x.Attributes.Length > 0);

            var res = new List<KeyValuePair<RouteAttribute, MethodInfo>>();

            foreach (var item in mehotdsAndTheirAttributes)
            {
                foreach (var attr in item.Attributes)
                {
                    res.Add(new KeyValuePair<RouteAttribute, MethodInfo>(attr, item.MethodInfo));
                }
            }

            return res.ToArray();
        }

        /// <summary>
        /// Returns the <c>instance</c> property of the given <paramref name="type"/> regardless of it's access modifier.
        /// <remarks>This method can be used to return both a <c>public</c> or <c>non-public</c> property.</remarks>
        /// </summary>
        [DebuggerStepThrough]
        internal static bool TryGetInstanceProperty(this Type type, string propertyName, out PropertyInfo property, bool inherit = true)
        {
            Ensure.NotNull(type, nameof(type));
            Ensure.NotNullOrEmptyOrWhiteSpace(propertyName);

            var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            if (!inherit) { flags = flags | BindingFlags.DeclaredOnly; }

            property = type.GetProperties(flags)
                .FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.Ordinal));

            return property != null;
        }

        /// <summary>
        /// Returns all <c>instance</c> properties of the given <paramref name="type"/> regardless of it's access modifier.
        /// <remarks>This method can be used to return both a <c>public</c> or <c>non-public</c> property.</remarks>
        /// </summary>
        [DebuggerStepThrough]
        internal static IEnumerable<PropertyInfo> GetInstanceProperties(this Type type, bool inherit = true)
        {
            Ensure.NotNull(type, nameof(type));

            var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            if (!inherit) { flags = flags | BindingFlags.DeclaredOnly; }

            return type.GetProperties(flags);
        }
    }
}