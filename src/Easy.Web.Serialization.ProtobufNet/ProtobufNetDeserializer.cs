namespace Easy.Web.Serialization.ProtobufNet
{
    using System.IO;
    using System.Text;
    using Core.Interfaces;
    using Core.Extensions;
    using ProtoBuf;

    /// <summary>
    /// Provides an abstraction for <c>protobuf-net</c>.
    /// </summary>
    public sealed class ProtobufNetDeserializer : IDeserializer
    {
        /// <summary>
        /// De-serializes the given <paramref name="input"/> to the given <typeparamref name="T"/>.
        /// </summary>
        public T Deserialize<T>(Stream input, Encoding encoding)
        {
            if (input.CanSeek) { input.Position = 0; }
            return Serializer.Deserialize<T>(input);
        }

        /// <summary>
        /// Indicates whether this instance supports deserialization from the given <paramref name="mediaType"/>.
        /// </summary>
        public bool CanDeserialize(string mediaType)
        {
            return mediaType.IsBinaryMediaType();
        }
    }
}