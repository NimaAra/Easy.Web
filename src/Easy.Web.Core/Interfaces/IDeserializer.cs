namespace Easy.Web.Core.Interfaces
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Specifies the contract to be implemented by a de-serializer.
    /// </summary>
    public interface IDeserializer
    {
        /// <summary>
        /// De-serializes the given <paramref name="input"/> to the given <typeparamref name="T"/>.
        /// </summary>
        T Deserialize<T>(Stream input, Encoding encoding);

        /// <summary>
        /// Indicates whether this instance supports deserialization from the given <paramref name="mediaType"/>.
        /// </summary>
        bool CanDeserialize(string mediaType);
    }
}