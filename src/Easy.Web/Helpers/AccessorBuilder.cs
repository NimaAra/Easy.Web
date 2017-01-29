namespace Easy.Web.Core.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;
    using Easy.Web.Core.Extensions;

    /// <summary>
    /// Provides a very fast and efficient property setter and getter access as well as object creation.
    /// </summary>
    internal static class AccessorBuilder
    {
        /// <summary>
        /// Builds an <see cref="Accessor{TInstance}"/> which provides easy access to all of the <see cref="PropertyInfo"/> of the given <typeparamref name="TInstance"/>.
        /// </summary>
        [DebuggerStepThrough]
        public static Accessor<TInstance> Build<TInstance>(bool ignoreCase = false, bool includeNonPublic = false) where TInstance : class
        {
            return new Accessor<TInstance>(ignoreCase, includeNonPublic);
        }

        /// <summary>
        /// Builds an <see cref="StringAccessor{TInstance}"/> which provides easy access to all of the <see cref="PropertyInfo"/> of the given <typeparamref name="TInstance"/>.
        /// </summary>
        [DebuggerStepThrough]
        public static StringAccessor<TInstance> BuildForValueAsString<TInstance>(bool ignoreCase = false, bool includeNonPublic = false) where TInstance : class
        {
            return new StringAccessor<TInstance>(ignoreCase, includeNonPublic);
        }

        /// <summary>
        /// Builds a property setter for a given instance type of <typeparamref name="TInstance"/> and property type of <typeparamref name="TProperty"/> with the name of <paramref name="propertyName"/>.
        /// <remarks>
        /// The setters for a <typeparamref name="TInstance"/> of <see lang="struct"/> are 
        /// intentionally not supported as changing the values of immutable types is a bad practice.
        /// </remarks>
        /// </summary>
        [DebuggerStepThrough]
        public static Action<TInstance, TProperty> BuildSetter<TInstance, TProperty>(string propertyName, bool includeNonPublic = false) where TInstance : class
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(propertyName);

            PropertyInfo propInfo;
            var found = typeof(TInstance).TryGetInstanceProperty(propertyName, out propInfo);
            Ensure.That<InvalidOperationException>(found, "Unable to find property: " + propertyName + ".");
            return BuildSetter<TInstance, TProperty>(propInfo, includeNonPublic);
        }

        /// <summary>
        /// Builds a property setter for a given instance type of <typeparamref name="TInstance"/> and property type of <typeparamref name="TProperty"/>.
        /// <remarks>
        /// The setters for a <typeparamref name="TInstance"/> of <see lang="struct"/> are 
        /// intentionally not supported as changing the values of immutable types is a bad practice.
        /// </remarks>
        /// </summary>
        [DebuggerStepThrough]
        public static Action<TInstance, TProperty> BuildSetter<TInstance, TProperty>(PropertyInfo propertyInfo, bool includeNonPublic = false) where TInstance : class
        {
            Ensure.NotNull(propertyInfo, nameof(propertyInfo));
            var setMethod = propertyInfo.GetSetMethod(includeNonPublic);
            return (Action<TInstance, TProperty>)setMethod.CreateDelegate(typeof(Action<TInstance, TProperty>), setMethod);
        }

        /// <summary>
        /// Builds a property getter for a given instance type of <typeparamref name="TInstance"/> and property type of <typeparamref name="TProperty"/> with the name of <paramref name="propertyName"/>.
        /// </summary>
        [DebuggerStepThrough]
        public static Func<TInstance, TProperty> BuildGetter<TInstance, TProperty>(string propertyName, bool includeNonPublic = false) where TInstance : class
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(propertyName);

            PropertyInfo propInfo;
            var found = typeof(TInstance).TryGetInstanceProperty(propertyName, out propInfo);
            Ensure.That<InvalidOperationException>(found, "Unable to find property: " + propertyName + ".");
            return BuildGetter<TInstance, TProperty>(propInfo, includeNonPublic);
        }

        /// <summary>
        /// Builds a property getter for a given instance type of <typeparamref name="TInstance"/> and property type of <typeparamref name="TProperty"/>.
        /// </summary>
        [DebuggerStepThrough]
        public static Func<TInstance, TProperty> BuildGetter<TInstance, TProperty>(PropertyInfo propertyInfo, bool includeNonPublic = false) where TInstance : class
        {
            Ensure.NotNull(propertyInfo, nameof(propertyInfo));
            var getMethod = propertyInfo.GetGetMethod(includeNonPublic);
            return (Func<TInstance, TProperty>)getMethod.CreateDelegate(typeof(Func<TInstance, TProperty>), getMethod);
        }

        /// <summary>
        /// Builds a property setter for a given instance type of <typeparamref name="TInstance"/> and property name of <paramref name="propertyName"/>.
        /// <remarks>
        /// The setters for a <typeparamref name="TInstance"/> of <see lang="struct"/> are 
        /// intentionally not supported as changing the values of immutable types is a bad practice.
        /// </remarks>
        /// </summary>
        [DebuggerStepThrough]
        public static Action<TInstance, object> BuildSetter<TInstance>(string propertyName, bool includeNonPublic = false) where TInstance : class
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(propertyName);

            PropertyInfo propInfo;
            var found = typeof(TInstance).TryGetInstanceProperty(propertyName, out propInfo);
            Ensure.That<InvalidOperationException>(found, "Unable to find property: " + propertyName + ".");
            return BuildSetter<TInstance>(propInfo, includeNonPublic);
        }

        /// <summary>
        /// Builds a property setter for a given instance type of <typeparamref name="TInstance"/> and property of <paramref name="propertyInfo"/>.
        /// <remarks>
        /// The setters for a <typeparamref name="TInstance"/> of <see lang="struct"/> are 
        /// intentionally not supported as changing the values of immutable types is a bad practice.
        /// </remarks>
        /// </summary>
        [DebuggerStepThrough]
        public static Action<TInstance, object> BuildSetter<TInstance>(PropertyInfo propertyInfo, bool includeNonPublic = false) where TInstance : class
        {
            Ensure.NotNull(propertyInfo, nameof(propertyInfo));
            var setMethod = propertyInfo.GetSetMethod(includeNonPublic);

            var instance = Expression.Parameter(typeof(TInstance), "instance");
            var value = Expression.Parameter(typeof(object), "value");
            var isValueType = propertyInfo.PropertyType.GetTypeInfo().IsValueType;
            var valueCast = !isValueType
                ? Expression.TypeAs(value, propertyInfo.PropertyType)
                : Expression.Convert(value, propertyInfo.PropertyType);

            return Expression.Lambda<Action<TInstance, object>>(
                Expression.Call(instance, setMethod, valueCast), instance, value).Compile();
        }

        /// <summary>
        /// Builds a property getter for a given instance type of <typeparamref name="TInstance"/> and property name of <paramref name="propertyName"/>.
        /// </summary>
        [DebuggerStepThrough]
        public static Func<TInstance, object> BuildGetter<TInstance>(string propertyName, bool includeNonPublic = false)
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(propertyName);

            PropertyInfo propInfo;
            var found = typeof(TInstance).TryGetInstanceProperty(propertyName, out propInfo);
            Ensure.That<InvalidOperationException>(found, "Unable to find property: " + propertyName + ".");
            return BuildGetter<TInstance>(propInfo, includeNonPublic);
        }

        /// <summary>
        /// Builds a property getter for a given instance type of <typeparamref name="TInstance"/> and property of <paramref name="propertyInfo"/>.
        /// </summary>
        [DebuggerStepThrough]
        public static Func<TInstance, object> BuildGetter<TInstance>(PropertyInfo propertyInfo, bool includeNonPublic = false)
        {
            Ensure.NotNull(propertyInfo, nameof(propertyInfo));
            var getMethod = propertyInfo.GetGetMethod(includeNonPublic);

            var instance = Expression.Parameter(typeof(TInstance), "instance");
            return Expression.Lambda<Func<TInstance, object>>(
                Expression.TypeAs(Expression.Call(instance, getMethod), typeof(object)), instance).Compile();
        }
    }
}
