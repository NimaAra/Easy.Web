namespace Easy.Web.Serialization.Jil
{
    using System.IO;
    using System.Text;
    using global::Jil;
    using Core.Interfaces;
    using Core.Extensions;

    /// <summary>
    /// Provides an abstraction for <c>Jil</c>.
    /// </summary>
    public sealed class JilSerializer : ISerializer
    {
        private readonly Options _settings;

        /// <summary>
        /// Creates an instance of the <see cref="JilSerializer"/>.
        /// </summary>
        public JilSerializer(Options settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Creates an instance of the <see cref="JilSerializer"/>.
        /// </summary>
        public JilSerializer()
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
                JSON.Serialize(payload, writer, _settings);
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