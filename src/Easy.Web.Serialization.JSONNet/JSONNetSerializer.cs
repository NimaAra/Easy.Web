namespace Easy.Web.Serialization.JSONNet
{
    using System.IO;
    using System.Text;
    using Core.Interfaces;
    using Core.Extensions;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides an abstraction for <c>JSON.NET</c>.
    /// </summary>
    public sealed class JSONNetSerializer : ISerializer
    {
        private readonly JsonSerializer _serializer;

        /// <summary>
        /// Creates an instance of the <see cref="JSONNetSerializer"/>.
        /// </summary>
        public JSONNetSerializer(JsonSerializerSettings settings)
        {
            _serializer = JsonSerializer.Create(settings);
        }

        /// <summary>
        /// Creates an instance of the <see cref="JSONNetSerializer"/>.
        /// </summary>
        public JSONNetSerializer()
        {
            _serializer = JsonSerializer.Create(Settings.Default);
        }

        /// <summary>
        /// Serializes the given <paramref name="payload"/> to the given <paramref name="output"/>.
        /// </summary>
        public void Serialize<T>(T payload, Stream output, Encoding encoding)
        {
            using (var writer = new StreamWriter(output, encoding, 512, true))
            {
                _serializer.Serialize(writer, payload, typeof(T));
            }
        }

        /// <summary>
        /// Indicates whether this instance supports serialization to given <paramref name="mediaType"/>.
        /// </summary>
        public bool CanSerialize(string mediaType)
        {
            return mediaType.IsJSONMediaType();
        }
    }
}