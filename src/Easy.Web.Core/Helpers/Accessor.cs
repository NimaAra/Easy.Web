namespace Easy.Web.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Easy.Web.Core.Extensions;

    /// <summary>
    /// An abstraction for gaining fast access to all of the <see cref="PropertyInfo"/> of the given <typeparamref name="TInstance"/>.
    /// </summary>
    internal class Accessor<TInstance> where TInstance : class
    {
        protected readonly Dictionary<string, Func<TInstance, object>> GenericPropertiesGettersCache;
        protected readonly Dictionary<string, Action<TInstance, object>> GenericPropertiesSettersCache;

        internal Accessor(bool ignoreCase, bool includeNonPublic)
        {
            Type = typeof(TInstance);
            IgnoreCase = ignoreCase;
            IncludesNonPublic = includeNonPublic;

            var flags = BindingFlags.Public | BindingFlags.Instance;
            if (IncludesNonPublic)
            {
                flags = flags | BindingFlags.NonPublic;
            }

            Properties = Type.GetProperties(flags);

            var comparer = IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            GenericPropertiesGettersCache = new Dictionary<string, Func<TInstance, object>>(Properties.Length, comparer);
            GenericPropertiesSettersCache = new Dictionary<string, Action<TInstance, object>>(Properties.Length, comparer);

            foreach (var prop in Properties)
            {
                var propName = prop.Name;
                GenericPropertiesGettersCache[propName] = AccessorBuilder.BuildGetter<TInstance>(prop, IncludesNonPublic);
                GenericPropertiesSettersCache[propName] = AccessorBuilder.BuildSetter<TInstance>(prop, IncludesNonPublic);
            }
        }

        /// <summary>
        /// Gets the type of the object this instance supports.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the flag indicating whether property names should be treated in a case sensitive manner.
        /// </summary>
        public bool IgnoreCase { get; }

        /// <summary>
        /// Gets the flag indicating whether non-public properties should be supported by this instance.
        /// </summary>
        public bool IncludesNonPublic { get; }

        /// <summary>
        /// Gets the properties to which this instance can provide access to.
        /// </summary>
        public PropertyInfo[] Properties { get; }

        /// <summary>
        /// Gets the value of the given <paramref name="propertyName"/> for the given <paramref name="instance"/>.
        /// </summary>
        public object Get(TInstance instance, string propertyName)
        {
            Func<TInstance, object> getter;
            GenericPropertiesGettersCache.TryGetValue(propertyName, out getter);
            // ReSharper disable once PossibleNullReferenceException
            return getter(instance);
        }

        /// <summary>
        /// Sets the value of the given <paramref name="propertyName"/> for the given <paramref name="instance"/>.
        /// </summary>
        public void Set(TInstance instance, string propertyName, object propValue)
        {
            Action<TInstance, object> setter;
            GenericPropertiesSettersCache.TryGetValue(propertyName, out setter);
            // ReSharper disable once PossibleNullReferenceException
            setter(instance, propValue);
        }
    }

    internal sealed class StringAccessor<TInstance> : Accessor<TInstance> where TInstance : class
    {
        public StringAccessor(bool ignoreCase, bool includeNonPublic) : base(ignoreCase, includeNonPublic)
        {
            GenericPropertiesGettersCache.Clear();
            GenericPropertiesSettersCache.Clear();

            foreach (var prop in Properties)
            {
                var propName = prop.Name;
                GenericPropertiesGettersCache[propName] = AccessorBuilder.BuildGetter<TInstance>(prop, IncludesNonPublic);

                var setter = AccessorBuilder.BuildSetter<TInstance>(prop, IncludesNonPublic);
                Action<TInstance, object> wrapper = (instance, value) =>
                {
                    object convertedValue;
                    if (((string)value).TryConvert(prop.PropertyType, out convertedValue))
                    {
                        setter(instance, convertedValue);
                    }
                };
                GenericPropertiesSettersCache[propName] = wrapper;
            }
        }
    }
}