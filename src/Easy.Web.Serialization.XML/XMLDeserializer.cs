namespace Easy.Web.Serialization.XML
{
    using System.IO;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Xml.Linq;
    using Core.Interfaces;
    using Core.Extensions;
    using Core.Binding;

    /// <summary>
    /// Provides an abstraction for <see cref="XmlSerializer"/>.
    /// </summary>
    public sealed class XMLDeserializer : IDeserializer
    {
        /// <summary>
        /// De-serializes the given <paramref name="input"/> to the given <typeparamref name="T"/>.
        /// </summary>
        public T Deserialize<T>(Stream input, Encoding encoding)
        {
            if (input.CanSeek) { input.Position = 0; }
            using (var reader = new StreamReader(input, encoding, false, 512, true))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Indicates whether this instance supports deserialization from the given <paramref name="mediaType"/>.
        /// </summary>
        public bool CanDeserialize(string mediaType)
        {
            return mediaType.IsXMLMediaType();
        }

        /// <summary>
        /// Converts the content of the given <paramref name="reader"/> to <see cref="DynamicDictionary"/>.
        /// </summary>
        private static DynamicDictionary ToDynamic(TextReader reader, bool ignoreCase = true)
        {
            var result = new DynamicDictionary(ignoreCase);
            var elements = new List<XElement>();
            result["Elements"] = elements;

            using (var xmlReader = XmlReader.Create(reader))
            {
                xmlReader.MoveToElement();
                while (xmlReader.Read())
                {
                    while (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        var element = (XElement)XNode.ReadFrom(xmlReader);
                        elements.Add(element);
                    }
                }

                return result;
            }
        }
    }
}