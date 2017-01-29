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
    public sealed class ProtobufNetSerializer : ISerializer
    {
        /// <summary>
        /// Serializes the given <paramref name="payload"/> to the given <paramref name="output"/>.
        /// </summary>
        public void Serialize<T>(T payload, Stream output, Encoding encoding)
        {
            Serializer.Serialize(output, payload);
        }

        /// <summary>
        /// Indicates whether this instance supports serialization to given <paramref name="mediaType"/>.
        /// </summary>
        public bool CanSerialize(string mediaType)
        {
            return mediaType.IsBinaryMediaType();
        }
    }
}