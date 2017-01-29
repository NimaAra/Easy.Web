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
    public sealed class JilDeserializer : IDeserializer
    {
        private readonly Options _settings;

        /// <summary>
        /// Creates an instance of the <see cref="JilDeserializer"/>.
        /// </summary>
        public JilDeserializer(Options settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Creates an instance of the <see cref="JilDeserializer"/>.
        /// </summary>
        public JilDeserializer()
        {
            _settings = Settings.Default;
        }

        /// <summary>
        /// De-serializes the given <paramref name="input"/> to the given <typeparamref name="T"/>.
        /// </summary>
        public T Deserialize<T>(Stream input, Encoding encoding)
        {
            if (input.CanSeek) { input.Position = 0; }
            using (var reader = new StreamReader(input, encoding, false, 512, true))
            {
                return JSON.Deserialize<T>(reader, _settings);
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