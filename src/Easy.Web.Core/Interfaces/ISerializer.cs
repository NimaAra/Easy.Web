namespace Easy.Web.Core.Interfaces
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Specifies the contract to be implemented by a serializer.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes the given <paramref name="payload"/> to the given <paramref name="output"/>.
        /// </summary>
        void Serialize<T>(T payload, Stream output, Encoding encoding);

        /// <summary>
        /// Indicates whether this instance supports serialization to given <paramref name="mediaType"/>.
        /// </summary>
        bool CanSerialize(string mediaType);
    }
}