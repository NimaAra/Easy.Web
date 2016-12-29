namespace Easy.Web.Core.Models
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a list of <c>HTTP</c> request headers.
    /// <remarks>Referenced from: <see href="https://en.wikipedia.org/wiki/List_of_HTTP_header_fields"/>.</remarks>
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class HttpRequestHeaders
    {
        /// <summary>
        /// <c>Content-Types</c> that are acceptable for the response. <see href="https://en.wikipedia.org/wiki/Content_negotiation"/>.
        /// <example><code>Accept: text/plain</code></example>
        /// </summary>
        public const string Accept = "Accept";

        /// <summary>
        /// Character sets that are acceptable.
        /// <example><code>Accept-Charset: utf-8</code></example>
        /// </summary>
        public const string AcceptCharset = "Accept-Charset";

        /// <summary>
        /// List of acceptable encodings. <see href="https://en.wikipedia.org/wiki/HTTP_compression"/>.
        /// <example><code>Accept-Encoding: gzip, deflate</code></example>
        /// </summary>
        public const string AcceptEncoding = "Accept-Encoding";

        /// <summary>
        /// List of acceptable human languages for response. <see href="https://en.wikipedia.org/wiki/Content_negotiation"/>.
        /// <example><code>Accept-Language: en-US</code></example>
        /// </summary>
        public const string AcceptLanguage = "Accept-Language";

        /// <summary>
        /// Authentication credentials for <c>HTTP</c> authentication.
        /// <example><code>Authorization: Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==</code></example>
        /// </summary>
        public const string Authorization = "Authorization";

        /// <summary>
        /// Used to specify directives that must be obeyed by all caching mechanisms along the request-response chain.
        /// <example><code>Cache-Control: no-cache</code></example>
        /// </summary>
        public const string CacheControl = "Cache-Control";

        /// <summary>
        /// An <c>HTTP</c> cookie previously sent by the server with <c>Set-Cookie</c>
        /// <example><code>Cookie: $Version=1; Skin=new;</code></example>
        /// </summary>
        public const string Cookie = "Cookie";

        /// <summary>
        /// Control options for the current connection and list of hop-by-hop request fields.
        /// <example><code>Connection: keep-alive</code></example>
        /// <example><code>Connection: Upgrade</code></example>
        /// </summary>
        public const string Connection = "Connection";

        /// <summary>
        /// The length of the request body in octets (8-bit bytes).
        /// <example><code>Content-Length: 348</code></example>
        /// </summary>
        public const string ContentLength = "Content-Length";

        /// <summary>
        /// The <c>MIME</c> type of the body of the request (used with <c>POST</c> and <c>PUT</c> requests).
        /// <example><code>Content-Type: application/x-www-form-urlencoded</code></example>
        /// </summary>
        public const string ContentType = "Content-Type";

        /// <summary>
        /// The date and time that the message was originated (in <c>"HTTP-date"</c> format as defined by <c>RFC 7231</c> Date/Time Formats.
        /// <example><code>Date: Tue, 15 Nov 1994 08:12:31 GMT</code></example>
        /// </summary>
        public const string Date = "Date";

        /// <summary>
        /// This is the address of the previous web page from which a link to the currently requested page was followed. 
        /// (The word <c>“referrer”</c> has been misspelled in the RFC as well as in most implementations 
        /// to the point that it has become standard usage and is considered correct terminology).
        /// <example><code>Referer: http://en.wikipedia.org/wiki/Main_Page</code></example>
        /// </summary>
        public const string Referer = "Referer";

        /// <summary>
        /// Authorization credentials for connecting to a proxy.
        /// <example><code>Proxy-Authorization: Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==</code></example>
        /// </summary>
        public const string ProxyAuthorization = "Proxy-Authorization";

        /// <summary>
        /// The user agent string of the user agent.
        /// <example><code>User-Agent: Mozilla/5.0 (X11; Linux x86_64; rv:12.0) Gecko/20100101 Firefox/21.0</code></example>
        /// </summary>
        public const string UserAgent = "User-Agent";

        /// <summary>
        /// Initiates a request for cross-origin resource sharing (asks server for an <c>"Access-Control-Allow-Origin"</c> response field).
        /// <example><code>Origin: http://www.example-social-network.com</code></example>
        /// </summary>
        public const string Origin = "Origin";

        /// <summary>
        /// The domain name of the server (for virtual hosting), and the <c>TCP</c> port number on which the server is listening. 
        /// The port number may be omitted if the port is the standard port for the service requested.
        /// <remarks>Mandatory since <c>HTTP/1.1</c></remarks>
        /// <example><code>Host: en.wikipedia.org:8080</code></example>
        /// <example><code>Host: en.wikipedia.org</code></example>
        /// </summary>
        public const string Host = "Host";

        /// <summary>
        /// Only perform the action if the client supplied entity matches the same entity on the server. 
        /// This is mainly for methods like <c>PUT</c> to only update a resource if it has not been modified since the user last updated it.
        /// <example><code>If-Match: "737060cd8c284d8af7ad3082f209582d"</code></example>
        /// </summary>
        public const string IfMatch = "If-Match";

        /// <summary>
        /// Allows a <c>304 Not Modified</c> to be returned if content is unchanged.
        /// <example><code>If-Modified-Since: Sat, 29 Oct 1994 19:43:31 GMT</code></example>
        /// </summary>
        public const string IfModifiedSince = "If-Modified-Since";

        /// <summary>
        /// Allows a 304 Not Modified to be returned if content is unchanged. <see href="https://en.wikipedia.org/wiki/HTTP_ETag"/>.
        /// <example><code>If-None-Match: "737060cd8c284d8af7ad3082f209582d"</code></example>
        /// </summary>
        public const string IfNoneMatch = "If-None-Match";

        /// <summary>
        /// If the entity is unchanged, send me the part(s) that I am missing; otherwise, send me the entire new entity.
        /// <example><code>If-Range: "737060cd8c284d8af7ad3082f209582d"</code></example>
        /// </summary>
        public const string IfRange = "If-Range";

        /// <summary>
        /// Only send the response if the entity has not been modified since a specific time.
        /// <example><code>If-Unmodified-Since: Sat, 29 Oct 1994 19:43:31 GMT</code></example>
        /// </summary>
        public const string IfUnmodifiedSince = "If-Unmodified-Since";

        /// <summary>
        /// Limit the number of times the message can be forwarded through proxies or gateways.
        /// <example><code>Max-Forwards: 10</code></example>
        /// </summary>
        public const string MaxForwards = "Max-Forwards";

        /// <summary>
        /// The email address of the user making the request.
        /// <example><code>From: user@example.com</code></example>
        /// </summary>
        public const string From = "From";

        /// <summary>
        /// Disclose original information of a client connecting to a web server through an <c>HTTP</c> proxy.
        /// <example><code>Forwarded: for=192.0.2.60;proto=http;by=203.0.113.43</code></example>
        /// <example><code>Forwarded: for=192.0.2.43, for=198.51.100.17</code></example>
        /// </summary>
        public const string Forwarded = "Forwarded";

        /// <summary>
        /// Implementation-specific fields that may have various effects anywhere along the request-response chain.
        /// <example><code>Pragma: no-cache</code></example>
        /// </summary>
        public const string Pragma = "Pragma";

        /// <summary>
        /// Ask the server to upgrade to another protocol.
        /// <example><code>Upgrade: HTTP/2.0, HTTPS/1.3, IRC/6.9, RTA/x11, websocket</code></example>
        /// </summary>
        public const string Upgrade = "Upgrade";

        /// <summary>
        /// Informs the server of proxies through which the request was sent.
        /// <example><code>Via: 1.0 fred, 1.1 example.com (Apache/1.1)</code></example>
        /// </summary>
        public const string Via = "Via";

        /// <summary>
        /// A general warning about possible problems with the entity body.
        /// <example><code>Warning: 199 Miscellaneous warning</code></example>
        /// </summary>
        public const string Warning = "Warning";
    }
}