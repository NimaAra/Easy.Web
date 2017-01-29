namespace Easy.Web.Serialization.JSONNet
{
    using System.IO;
    using System.Text;
    using NetJSON;
    using Core.Interfaces;
    using Core.Extensions;

    /// <summary>
    /// Provides an abstraction for <c>Net JSON</c>.
    /// </summary>
    public sealed class NetJSONSerializer : ISerializer
    {
        private readonly NetJSONSettings _settings;

        /// <summary>
        /// Creates an instance of the <see cref="NetJSONSerializer"/>.
        /// </summary>
        public NetJSONSerializer(NetJSONSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Creates an instance of the <see cref="NetJSONSerializer"/>.
        /// </summary>
        public NetJSONSerializer()
        {
            _settings = Settings.Default;
        }

        /// <summary>
        /// Serializes the given <paramref name="payload"/> to the given <paramref name="output"/>.
        /// </summary>
        public void Serialize<T>(T payload, Stream output, Encoding encoding)
        {
            using (var writer = new StreamWriter(output, encoding, 512, true))
            {
                NetJSON.Serialize(payload, writer, _settings);
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