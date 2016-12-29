// ReSharper disable LoopCanBeConvertedToQuery
namespace Easy.Web.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
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
    }
}