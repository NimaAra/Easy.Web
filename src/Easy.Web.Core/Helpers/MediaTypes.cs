// ReSharper disable InconsistentNaming
namespace Easy.Web.Core.Helpers
{
    /// <summary>
    /// Represents most common media types.
    /// <remarks>Referenced from: <see href="http://www.iana.org/assignments/media-types/media-types.xhtml"/>.</remarks>
    /// </summary>
    public static class MediaTypes
    {
        /// <summary>
        /// <c>text/html</c>.
        /// </summary>
        public const string TEXT = "text/plain";

        /// <summary>
        /// <c>application/json</c>
        /// </summary>
        public const string JSON = "application/json";

        /// <summary>
        /// <c>application/xml</c>
        /// </summary>
        public const string XML = "application/xml";

        /// <summary>
        /// <c>text/html</c>
        /// </summary>
        public const string HTML = "text/html";

        /// <summary>
        /// <c>text/css</c>
        /// </summary>
        public const string CSS = "text/css";

        /// <summary>
        /// <c>application/javascript</c>
        /// </summary>
        public const string JavaScript = "application/javascript";

        /// <summary>
        /// <c>application/pdf</c>
        /// </summary>
        public const string PDF = "application/pdf";

        /// <summary>
        /// <c>application/octet-stream</c>
        /// </summary>
        public const string Binary = "application/octet-stream";

        /// <summary>
        /// <c>multipart/form-data</c>
        /// </summary>
        public const string FormData = "multipart/form-data";

        /// <summary>
        /// <c>application/x-www-form-urlencoded</c>
        /// </summary>
        public const string FormUrlEncoded = "application/x-www-form-urlencoded";

        /// <summary>
        /// <c>application/zip</c>
        /// </summary>
        public const string ZIP = "application/zip";

        /// <summary>
        /// <c>image/gif</c>
        /// </summary>
        public const string GIF = "image/gif";

        /// <summary>
        /// <c>image/jpeg</c>
        /// </summary>
        public const string JPEG = "image/jpeg";

        /// <summary>
        /// <c>image/png</c>
        /// </summary>
        public const string PNG = "image/png";
    }
}