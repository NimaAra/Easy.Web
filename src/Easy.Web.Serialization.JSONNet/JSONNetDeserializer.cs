namespace Easy.Web.Serialization.JSONNet
{
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using Core.Interfaces;
    using Core.Extensions;

    /// <summary>
    /// Provides an abstraction for <c>JSON.NET</c>.
    /// </summary>
    public sealed class JSONNetDeserializer : IDeserializer
    {
        private readonly JsonSerializer _serializer;

        /// <summary>
        /// Creates an instance of the <see cref="JSONNetDeserializer"/>.
        /// </summary>
        public JSONNetDeserializer(JsonSerializerSettings settings)
        {
            _serializer = JsonSerializer.Create(settings);
        }

        /// <summary>
        /// Creates an instance of the <see cref="JSONNetDeserializer"/>.
        /// </summary>
        public JSONNetDeserializer()
        {
            _serializer = JsonSerializer.Create(Settings.Default);
        }

        /// <summary>
        /// De-serializes the given <paramref name="input"/> to the given <typeparamref name="T"/>.
        /// </summary>
        public T Deserialize<T>(Stream input, Encoding encoding)
        {
            if (input.CanSeek) { input.Position = 0; }
            using (var reader = new StreamReader(input, encoding, false, 512, true))
            {
                return (T)_serializer.Deserialize(reader, typeof(T));
            }
        }

        /// <summary>
        /// Indicates whether this instance supports deserialization from the given <paramref name="mediaType"/>.
        /// </summary>
        public bool CanDeserialize(string mediaType)
        {
            return mediaType.IsJSONMediaType();
        }
    }
}