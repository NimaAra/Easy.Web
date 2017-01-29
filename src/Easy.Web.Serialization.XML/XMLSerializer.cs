namespace Easy.Web.Serialization.XML
{
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Core.Interfaces;
    using Core.Extensions;

    /// <summary>
    /// Provides an abstraction for <see cref="XmlSerializer"/>.
    /// </summary>
    public sealed class XMLSerializer : ISerializer
    {
        /// <summary>
        /// Serializes the given <paramref name="payload"/> to the given <paramref name="output"/>.
        /// </summary>
        public void Serialize<T>(T payload, Stream output, Encoding encoding)
        {
            using (var writer = new StreamWriter(output, encoding, 512, true))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, payload);
            }
        }

        /// <summary>
        /// Indicates whether this instance supports serialization to given <paramref name="mediaType"/>.
        /// </summary>
        public bool CanSerialize(string mediaType)
        {
            return mediaType.IsXMLMediaType();
        }
    }
}