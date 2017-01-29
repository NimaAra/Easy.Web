namespace Easy.Web.Core.Models
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Enumerates different <c>HTTP</c> verbs.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum HttpMethod
    {
        /// <summary>
        /// The <c>GET</c> method.
        /// </summary>
        GET = 0,

        /// <summary>
        /// The <c>POST</c> method.
        /// </summary>
        POST,

        /// <summary>
        /// The <c>PUT</c> method.
        /// </summary>
        PUT,

        /// <summary>
        /// The <c>DELETE</c> method.
        /// </summary>
        DELETE,

        /// <summary>
        /// The <c>HEAD</c> method.
        /// </summary>
        HEAD,

        /// <summary>
        /// The <c>OPTIONS</c> method.
        /// </summary>
        OPTIONS,
        
        /// <summary>
        /// The <c>CONNECT</c> method.
        /// </summary>
        CONNECT,

        /// <summary>
        /// The <c>PATCH</c> method.
        /// </summary>
        PATCH
    }
}