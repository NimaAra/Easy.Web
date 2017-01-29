﻿namespace Easy.Web.Core.Binding
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Dynamic;

    /// <summary>
    /// Provides an abstraction for an object to be used dynamically as a key value pair
    /// where the property is the key and value is an <see cref="object"/>.
    /// </summary>
    public sealed class DynamicDictionary : DynamicObject, IDictionary<string, object>
    {
        private readonly IDictionary<string, object> _dictionary;

        /// <summary>
        /// Creates a new instance of <see cref="DynamicDictionary"/>.
        /// </summary>
        /// <param name="ignoreCase">
        /// The flag indicating whether keys should be treated regardless of the case.
        /// </param>
        [DebuggerStepThrough]
        public DynamicDictionary(bool ignoreCase = true)
        {
            _dictionary = new Dictionary<string, object>(
                ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        }

        /// <summary>
        /// Add the given <paramref name="item"/> to this instance.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, object> item)
        {
            _dictionary.Add(item);
        }

        /// <summary>
        /// Removes all the items from this instance.
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Determines whether this instance contains the given <paramref name="item"/>.
        /// </summary>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dictionary.Contains(item);
        }

        /// <summary>
        /// Copies the elements of this instance to the given <paramref name="array"/>, starting at a particular <paramref name="array"/>.
        /// </summary>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the given <paramref name="item"/> from this instance.
        /// </summary>
        public bool Remove(KeyValuePair<string, object> item)
        {
            return _dictionary.Remove(item);
        }

        /// <summary>
        /// Gets the number of elements contained in this instance.
        /// </summary>
        public int Count => _dictionary.Keys.Count;

        /// <summary>
        /// Determines whether this instance is <c>Read-Only</c>.
        /// </summary>
        public bool IsReadOnly => _dictionary.IsReadOnly;

        /// <summary>
        /// Returns an enumerator that iterates through the keys and values of this instance.
        /// </summary>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the keys and values of this instance.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Determines whether this instance contains an element with the given <paramref name="key"/>.
        /// </summary>
        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Adds an element for the given <paramref name="key"/> and associated <paramref name="value"/> to this instance.
        /// </summary>
        public void Add(string key, object value)
        {
            _dictionary.Add(key, value);
        }

        /// <summary>
        /// Removes the element with the given <paramref name="key"/> from this instance.
        /// </summary>
        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        /// <summary>
        /// Attempts to get the value associated to the given <paramref name="key"/>.
        /// </summary>
        public bool TryGetValue(string key, out object value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets or sets the value stored against the given <paramref name="key"/>.
        /// <remarks>If the given <paramref name="key"/> does not exist, <c>NULL</c> is returned.</remarks>
        /// </summary>
        public object this[string key]
        {
            get
            {
                object result;
                _dictionary.TryGetValue(key, out result);
                return result;
            }

            set { _dictionary[key] = value; }
        }

        /// <summary>
        /// Gets an <see cref="ICollection{String}"/> containing the keys of this instance.
        /// </summary>
        public ICollection<string> Keys => _dictionary.Keys;

        /// <summary>
        /// Gets an <see cref="ICollection{Object}"/> containing the values of this instance.
        /// </summary>
        public ICollection<object> Values => _dictionary.Values;

        /// <summary>
        /// Provides the implementation for operations that get member values. 
        /// Classes derived from the <see cref="DynamicObject"/> class can override this method to 
        /// specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_dictionary.ContainsKey(binder.Name))
            {
                result = _dictionary[binder.Name];
                return true;
            }

            if (base.TryGetMember(binder, out result))
            {
                return true;
            }

            // always return null if not found.
            result = null;
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that set member values. 
        /// Classes derived from the <see cref="DynamicObject"/> class can override this method to 
        /// specify dynamic behavior for operations such as setting a value for a property.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TrySetMember(SetMemberBinder binder, object result)
        {
            if (!_dictionary.ContainsKey(binder.Name)) { _dictionary.Add(binder.Name, result); }
            else { _dictionary[binder.Name] = result; }
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that invoke a member. 
        /// Classes derived from the <see cref="DynamicObject"/> class can override this method to 
        /// specify dynamic behavior for operations such as calling a method.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (_dictionary.ContainsKey(binder.Name) && _dictionary[binder.Name] is Delegate)
            {
                var del = (Delegate)_dictionary[binder.Name];
                result = del.DynamicInvoke(args);
                return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        /// <summary>
        /// Provides the implementation for operations that delete an object member. 
        /// This method is not intended for use in C# or Visual Basic.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            if (_dictionary.ContainsKey(binder.Name))
            {
                _dictionary.Remove(binder.Name);
                return true;
            }

            return base.TryDeleteMember(binder);
        }
    }
}