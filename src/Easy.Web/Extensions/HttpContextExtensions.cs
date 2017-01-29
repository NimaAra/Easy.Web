namespace Easy.Web.Core.Extensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Primitives;
    using Binding;
    using Helpers;
    using Interfaces;

    /// <summary>
    /// Provides a set of helper methods for dealing with the <see cref="HttpContext"/>.
    /// </summary>
    public static class HttpContextExtensions
    {
        private static readonly ConcurrentDictionary<string, ISerializer> SerializersCache = new ConcurrentDictionary<string, ISerializer>(StringComparer.OrdinalIgnoreCase);
        private static readonly ConcurrentDictionary<string, IDeserializer> DeserializersCache = new ConcurrentDictionary<string, IDeserializer>(StringComparer.OrdinalIgnoreCase);
        private static readonly ConcurrentDictionary<Type, object> AccessorsCache = new ConcurrentDictionary<Type, object>();
        private static readonly ConcurrentDictionary<Type, object> StringAccessorsCache = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Provides a reply for the given <paramref name="context"/> containing the <paramref name="payload"/> 
        /// as the <paramref name="mediaType"/> and the given <paramref name="statusCode"/> with the given <paramref name="encoding"/>.
        /// <remarks>
        /// If <paramref name="encoding"/> is passed as <c>NULL</c> a default encoding of <see cref="Encoding.UTF8"/> will be used.
        /// </remarks>
        /// </summary>
        public static Task Reply(this HttpContext context, string payload, string mediaType, HttpStatusCode statusCode, Encoding encoding = null)
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(payload);
            Ensure.NotNullOrEmptyOrWhiteSpace(mediaType);

            var resp = context.Response;

            resp.StatusCode = (int)statusCode;
            resp.Headers[HttpResponseHeaders.ContentLength] = payload.Length.ToString();
            resp.Headers[HttpResponseHeaders.ContentType] = mediaType;
            return resp.WriteAsync(payload, encoding ?? Encoding.UTF8);
        }

        /// <summary>
        /// Provides the <paramref name="statusCode"/> as the reply for the given <paramref name="context"/>.
        /// </summary>
        public static Task ReplyAsStatus(this HttpContext context, HttpStatusCode statusCode)
        {
            var resp = context.Response;
            resp.StatusCode = (int)statusCode;
            resp.Headers[HttpResponseHeaders.ContentLength] = "0";
            return resp.WriteAsync(string.Empty, Encoding.UTF8);
        }

        /// <summary>
        /// Provides a reply as <c>text/plain</c> for the given <paramref name="context"/> and the given <paramref name="statusCode"/>.
        /// </summary>
        public static Task ReplyAsText(this HttpContext context, string text, HttpStatusCode statusCode = HttpStatusCode.Okay)
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(text);

            // ReSharper disable once RedundantArgumentDefaultValue
            return Reply(context, text, MediaTypes.TEXT, statusCode);
        }

        /// <summary>
        /// Provides a reply as <see cref="MediaTypes.HTML"/> for the given <paramref name="context"/> and the given <paramref name="statusCode"/>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static Task ReplyAsHTML(this HttpContext context, string html, HttpStatusCode statusCode = HttpStatusCode.Okay)
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(html);

            // ReSharper disable once RedundantArgumentDefaultValue
            return Reply(context, html, MediaTypes.HTML, statusCode);
        }

        /// <summary>
        /// Provides a reply as <c>application/json</c> for the given <paramref name="context"/> and the given <paramref name="statusCode"/>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static Task ReplyAsJSON(this HttpContext context, string json, HttpStatusCode statusCode = HttpStatusCode.Okay)
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(json);

            // ReSharper disable once RedundantArgumentDefaultValue
            return Reply(context, json, MediaTypes.JSON, statusCode);
        }

        /// <summary>
        /// Provides a reply as <c>application/xml</c> for the given <paramref name="context"/> and the given <paramref name="statusCode"/>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static Task ReplyAsXML(this HttpContext context, string xml, HttpStatusCode statusCode = HttpStatusCode.Okay)
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(xml);

            return Reply(context, xml, MediaTypes.XML, statusCode);
        }

        /// <summary>
        /// Provides a reply for the given <paramref name="context"/> with the given <paramref name="payload"/> and the given <paramref name="statusCode"/>.
        /// </summary>
        public static Task ReplyAsStream(this HttpContext context, Stream payload, string mediaType, HttpStatusCode statusCode = HttpStatusCode.Okay)
        {
            Ensure.NotNull(payload, nameof(payload));
            Ensure.NotNullOrEmptyOrWhiteSpace(mediaType);

            var resp = context.Response;

            resp.StatusCode = (int)statusCode;

            payload.Position = 0;
            var length = payload.Length;
            resp.Headers[HttpResponseHeaders.ContentLength] = length.ToString();
            resp.Headers[HttpResponseHeaders.ContentType] = mediaType;

            return payload.CopyToAsync(resp.Body);
        }

        /// <summary>
        /// Provides a reply for the given <paramref name="context"/> with the given <paramref name="payload"/> and the given <paramref name="statusCode"/>.
        /// </summary>
        public static Task ReplyAsBinary(this HttpContext context, byte[] payload, string mediaType, HttpStatusCode statusCode = HttpStatusCode.Okay)
        {
            Ensure.NotNull(payload, nameof(payload));
            Ensure.NotNullOrEmptyOrWhiteSpace(mediaType);

            var resp = context.Response;

            resp.StatusCode = (int)statusCode;

            var length = payload.Length;
            resp.Headers[HttpResponseHeaders.ContentLength] = length.ToString();
            resp.Headers[HttpResponseHeaders.ContentType] = mediaType;

            return resp.Body.WriteAsync(payload, 0, length);
        }

        /// <summary>
        /// Provides a file attachment reply for the given <paramref name="context"/> with the given <paramref name="filePayload"/> and the given <paramref name="statusCode"/>.
        /// </summary>
        public static async Task ReplyAsFile(this HttpContext context, FileInfo filePayload, string mediaType, HttpStatusCode statusCode = HttpStatusCode.Okay)
        {
            Ensure.NotNull(filePayload, nameof(filePayload));
            Ensure.Exists(filePayload);
            Ensure.NotNullOrEmptyOrWhiteSpace(mediaType);

            var resp = context.Response;

            resp.StatusCode = (int)statusCode;

            var length = filePayload.Length;
            resp.Headers[HttpResponseHeaders.ContentDisposition] = "attachment; filename=" + filePayload.Name;
            resp.Headers[HttpResponseHeaders.ContentLength] = length.ToString();
            resp.Headers[HttpResponseHeaders.ContentType] = mediaType;

            var stream = File.OpenRead(filePayload.FullName);
            await stream.CopyToAsync(resp.Body).ConfigureAwait(false);
            stream.Dispose();
        }

        /// <summary>
        /// Serializes the given <paramref name="payload"/> as <c>JSON</c> and provides a reply to the given <paramref name="context"/> with the given <paramref name="statusCode"/>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static Task SerializeAsJSON<T>(this HttpContext context, T payload, HttpStatusCode statusCode = HttpStatusCode.Okay)
        {
            ISerializer jsonSerializer;
            if (!context.TryGetSerializer(MediaTypes.JSON, out jsonSerializer))
            {
                throw new NotSupportedException($"Unable to find an implementation of {nameof(ISerializer)} that supports {MediaTypes.JSON}.");
            }

            return Serialize(context, jsonSerializer, payload, MediaTypes.JSON, statusCode);
        }

        /// <summary>
        /// Serializes the given <paramref name="payload"/> as <c>XML</c> and provides a reply to the given <paramref name="context"/> with the given <paramref name="statusCode"/>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static Task SerializeAsXML<T>(this HttpContext context, T payload, HttpStatusCode statusCode = HttpStatusCode.Okay)
        {
            ISerializer xmlSerializer;
            if (!context.TryGetSerializer(MediaTypes.XML, out xmlSerializer))
            {
                throw new NotSupportedException($"Unable to find an implementation of {nameof(ISerializer)} that supports {MediaTypes.XML}.");
            }

            return Serialize(context, xmlSerializer, payload, MediaTypes.XML, statusCode);
        }

        /// <summary>
        /// Serializes the given <paramref name="payload"/> as <c>binary</c> and provides a reply to the given <paramref name="context"/> with the given <paramref name="statusCode"/>.
        /// </summary>
        public static Task SerializeAsBinary<T>(this HttpContext context, T payload, HttpStatusCode statusCode = HttpStatusCode.Okay)
        {
            ISerializer binarySerializer;
            if (!context.TryGetSerializer(MediaTypes.Binary, out binarySerializer))
            {
                throw new NotSupportedException($"Unable to find an implementation of {nameof(ISerializer)} that supports {MediaTypes.XML}.");
            }

            return Serialize(context, binarySerializer, payload, MediaTypes.Binary, statusCode);
        }

        /// <summary>
        /// Serializes and returns the given <paramref name="payload"/> to the client using <c>Content Negotiation</c>.
        /// <remarks>
        /// If no <c>Accept</c> header is found on the request or no <see cref="ISerializer"/> supporting the requested 
        /// <c>Accept</c> header has been registered, the <paramref name="payload"/> will be serialized as <c>application/json</c>.
        /// </remarks>
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Thrown if no <see cref="ISerializer"/> supporting <c>application/json</c> is found.
        /// </exception>
        public static Task Negotiate<T>(this HttpContext context, T payload, HttpStatusCode statusCode = HttpStatusCode.Okay)
        {
            var req = context.Request;

            StringValues acceptableMediaTypes;
            if (!req.Headers.TryGetValue(HttpRequestHeaders.Accept, out acceptableMediaTypes))
            {
                return SerializeAsJSON(context, payload, statusCode);
            }

            foreach (var rawMediaType in acceptableMediaTypes)
            {
                foreach (var mediaType in rawMediaType.ExtractMediaTypes())
                {
                    ISerializer serializer;
                    if (!TryGetSerializer(context, mediaType, out serializer)) { continue; }

                    return Serialize(context, serializer, payload, mediaType, statusCode);
                }
            }

            return SerializeAsJSON(context, payload, statusCode);
        }

        /// <summary>
        /// Attempts to obtain the <paramref name="serializer"/> for the given <paramref name="mediaType"/>.
        /// </summary>
        public static bool TryGetSerializer(this HttpContext context, string mediaType, out ISerializer serializer)
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(mediaType);

            if (SerializersCache.TryGetValue(mediaType, out serializer)) { return true; }

            var svcProvider = context.RequestServices;

            IEnumerable<ISerializer> serializers;
            try { serializers = svcProvider.GetServices<ISerializer>(); }
            catch (Exception) { return false; }

            foreach (var item in serializers)
            {
                if (!item.CanSerialize(mediaType)) { continue; }

                serializer = item;
                SerializersCache[mediaType] = serializer;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to obtain the <paramref name="deserializer"/> for the given <paramref name="mediaType"/>.
        /// </summary>
        public static bool TryGetDeserializer(this HttpContext context, string mediaType, out IDeserializer deserializer)
        {
            Ensure.NotNullOrEmptyOrWhiteSpace(mediaType);

            if (DeserializersCache.TryGetValue(mediaType, out deserializer)) { return true; }

            var svcProvider = context.RequestServices;

            IEnumerable<IDeserializer> serializers;
            try { serializers = svcProvider.GetServices<IDeserializer>(); }
            catch (Exception) { return false; }

            foreach (var item in serializers)
            {
                if (!item.CanDeserialize(mediaType)) { continue; }

                deserializer = item;
                DeserializersCache[mediaType] = deserializer;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reads all the values passed in via the query-string as key value to a <see cref="DynamicDictionary"/>. 
        /// </summary>
        public static DynamicDictionary ReadFromQueryString(this HttpContext context)
        {
            var queryCollection = context.Request.Query;

            var result = new DynamicDictionary();
            foreach (var item in queryCollection)
            {
                result[item.Key] = item.Value.ToString();
            }

            return result;
        }

        /// <summary>
        /// Reads all the values passed in via the query-string as key value to a given model of type <typeparamref name="T"/>.
        /// <remarks>The keys passed in the query-string are treated as the property name. The casing is ignored.</remarks>
        /// </summary>
        public static T ReadFromQueryString<T>(this HttpContext context) where T : class, new()
        {
            var queryCollection = context.Request.Query;
            var accessor = GetOrBuildPropertyAccessor<T>(true);
            var instance = new T();
            foreach (var item in queryCollection)
            {
                accessor.Set(instance, item.Key, item.Value.ToString());
            }

            return instance;
        }

        /// <summary>
        /// Reads the values passed in via the query-string as key value to a given model of type <typeparamref name="T"/> 
        /// and only for the property names specified by <paramref name="propertyNames"/>.
        /// <remarks>The keys passed in the query-string are treated as the property name. The casing is ignored.</remarks>
        /// </summary>
        public static T ReadFromQueryString<T>(this HttpContext context, params string[] propertyNames) where T : class, new()
        {
            var queryCollection = context.Request.Query;
            var accessor = GetOrBuildPropertyAccessor<T>(true);
            var instance = new T();
            foreach (var item in propertyNames)
            {
                StringValues value;
                if (!queryCollection.TryGetValue(item, out value)) { continue; }

                accessor.Set(instance, item, value.ToString());
            }

            return instance;
        }

        /// <summary>
        /// Reads the values passed inside the body of the request to the a given model of type <typeparamref name="T"/>.
        /// </summary>
        public static async Task<T> ReadFromBody<T>(this HttpContext context) where T : class, new()
        {
            var req = context.Request;
            var contentType = req.ContentType;
            
            if (contentType == null) { return null; }

            if (contentType.IsFormUrlEncodedMediaType())
            {
                var accessor = GetOrBuildPropertyAccessor<T>(true);

                var instance = new T();
                foreach (var item in await req.ReadFormAsync().ConfigureAwait(false))
                {
                    accessor.Set(instance, item.Key, item.Value.ToString());
                }
                return instance;
            }

            IDeserializer deserializer;
            if (!TryGetDeserializer(context, contentType, out deserializer))
            {
                throw new NotSupportedException($"Unable to find any {nameof(IDeserializer)} registered supporting {HttpRequestHeaders.ContentType} of: {contentType}.");
            }

            var encoding = GetEncoding(contentType);
            return deserializer.Deserialize<T>(req.Body, encoding);
        }

        /// <summary>
        /// Reads the values passed inside the body of the request to the a given model of type <typeparamref name="T"/>
        /// and only for the property names specified by <paramref name="propertyNames"/>.
        /// </summary>
        public static async Task<T> ReadFromBody<T>(this HttpContext context, params string[] propertyNames) where T : class, new()
        {
            var req = context.Request;
            var contentType = req.ContentType;

            if (contentType == null) { return null; }

            var accessor = GetOrBuildPropertyAccessor<T>(contentType.IsFormUrlEncodedMediaType());
            var tmpInstance = await ReadFromBody<T>(context);
            var instance = new T();

            foreach (var name in propertyNames)
            {
                var value = accessor.Get(tmpInstance, name);
                accessor.Set(instance, name, value);
            }

            return instance;
        }

        private static Task Serialize<T>(this HttpContext context, ISerializer serializer, T payload, string mediaType, HttpStatusCode statusCode)
        {
            using (var memStream = new MemoryStream())
            {
                serializer.Serialize(payload, memStream, Encoding.UTF8);
                return ReplyAsStream(context, memStream, mediaType, statusCode);
            }
        }

        private static Accessor<T> GetOrBuildPropertyAccessor<T>(bool valueAsString) where T : class, new()
        {
            var modelType = typeof(T);

            object accessor;

            if (valueAsString)
            {
                if (!StringAccessorsCache.TryGetValue(modelType, out accessor))
                {
                    accessor = AccessorBuilder.BuildForValueAsString<T>(true, true);
                    StringAccessorsCache[modelType] = accessor;
                }

                return (StringAccessor<T>)accessor;
            }
            
            if (!AccessorsCache.TryGetValue(modelType, out accessor))
            {
                accessor = AccessorBuilder.Build<T>(true, true);
                AccessorsCache[modelType] = accessor;
            }

            return (Accessor<T>)accessor;
        }

        private static Encoding GetEncoding(string contentType)
        {
            string charSet;
            if (!contentType.TryExtractCharacterSet(out charSet))
            {
                return Encoding.UTF8;
            }

            Encoding encoding;
            return !Encodings.TryGetEncoding(charSet, out encoding) ? Encoding.UTF8 : encoding;
        }
    }
}