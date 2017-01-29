// ReSharper disable InconsistentNaming
namespace Easy.Web.Core.Helpers
{
    /// <summary>
    /// Represents a list of <c>HTTP</c> response headers.
    /// <remarks>Referenced from: <see href="https://en.wikipedia.org/wiki/List_of_HTTP_header_fields"/>.</remarks>
    /// </summary>
    public static class HttpResponseHeaders
    {
        /// <summary>
        /// Specifying which web sites can participate in cross-origin resource sharing.
        /// <example><code>Access-Control-Allow-Origin: *</code></example>
        /// </summary>
        public const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

        /// <summary>
        /// What partial content range types this server supports via byte serving.
        /// <example><code>Accept-Ranges: bytes</code></example>
        /// </summary>
        public const string AcceptRanges = "Accept-Ranges";

        /// <summary>
        /// The age the object has been in a proxy cache in seconds.
        /// <example><code>Age: 12</code></example>
        /// </summary>
        public const string Age = "Age";

        /// <summary>
        /// The date and time that the message was sent (in <c>"HTTP-date"</c> format as defined by <c>RFC 7231</c>).
        /// <example><code>Date: Tue, 15 Nov 1994 08:12:31 GMT</code></example>
        /// </summary>
        public const string Date = "Date";

        /// <summary>
        /// An identifier for a specific version of a resource, often a message digest.
        /// <example><code>ETag: "737060cd8c284d8af7ad3082f209582d"</code></example>
        /// </summary>
        public const string ETag = "ETag";

        /// <summary>
        /// Gives the date/time after which the response is considered stale (in <c>"HTTP-date"</c> format as defined by <code>RFC 7231</code>.
        /// <example><code>Expires: Thu, 01 Dec 1994 16:00:00 GMT</code></example>
        /// </summary>
        public const string Expires = "Expires";

        /// <summary>
        /// The last modified date for the requested object (in <c>"HTTP-date"</c> format as defined by <c>RFC 7231</c>.
        /// <example><code>Last-Modified: Tue, 15 Nov 1994 12:45:26 GMT</code></example>
        /// </summary>
        public const string LastModified = "Last-Modified";

        /// <summary>
        /// Used to express a typed relationship with another resource, where the relation type is defined by <c>RFC 5988</c>.
        /// <example><code>Link: &lt;/feed&gt; rel="alternate"</code></example>
        /// </summary>
        public const string Link = "Link";

        /// <summary>
        /// Valid actions for a specified resource. To be used for a 405 Method not allowed.
        /// <example><code>Allow: GET, HEAD</code></example>
        /// </summary>
        public const string Allow = "Allow";

        /// <summary>
        /// Tells all caching mechanisms from server to client whether they may cache this object. It is measured in seconds.
        /// <example><code>Cache-Control: max-age=3600</code></example>
        /// </summary>
        public const string CacheControl = "Cache-Control";

        /// <summary>
        /// Control options for the current connection and list of hop-by-hop response fields.
        /// <example><code>Connection: close</code></example>
        /// </summary>
        public const string Connection = "Connection";

        /// <summary>
        /// An opportunity to raise a <c>"File Download"</c> dialogue box for a known <c>MIME</c> type with 
        /// binary format or suggest a filename for dynamic content. Quotes are necessary with special characters.
        /// <example><code>Content-Disposition: attachment; filename="fname.ext"</code></example>
        /// </summary>
        public const string ContentDisposition = "Content-Disposition";

        /// <summary>
        /// The type of encoding used on the data. <see href="https://en.wikipedia.org/wiki/HTTP_compression"/>.
        /// <example><code>Content-Encoding: gzip</code></example>
        /// </summary>
        public const string ContentEncoding = "Content-Encoding";

        /// <summary>
        /// The natural language or languages of the intended audience for the enclosed content.
        /// <example><code>Content-Language: da</code></example>
        /// </summary>
        public const string ContentLanguage = "Content-Language";

        /// <summary>
        /// The length of the response body in octets (8-bit bytes).
        /// <example><code>Content-Length: 348</code></example>
        /// </summary>
        public const string ContentLength = "Content-Length";

        /// <summary>
        /// An alternate location for the returned data.
        /// <example><code>Content-Location: /index.htm</code></example>
        /// </summary>
        public const string ContentLocation = "Content-Location";

        /// <summary>
        /// A <c>Base64-encoded</c> binary <c>MD5</c> sum of the content of the response.
        /// <example><code>Content-MD5: Q2hlY2sgSW50ZWdyaXR5IQ==</code></example>
        /// </summary>
        public const string ContentMD5 = "Content-MD5";

        /// <summary>
        /// Where in a full body message this partial message belongs.
        /// <example><code>Content-Range: bytes 21010-47021/47022</code></example>
        /// </summary>
        public const string ContentRange = "Content-Range";

        /// <summary>
        /// The <c>MIME</c> type of this content.
        /// <example><code>Content-Type: text/html; charset=utf-8</code></example>
        /// </summary>
        public const string ContentType = "Content-Type";

        /// <summary>
        /// Used in redirection, or when a new resource has been created.
        /// <example><code>Location: http://www.w3.org/pub/WWW/People.html</code></example>
        /// </summary>
        public const string Location = "Location";

        /// <summary>
        /// Implementation-specific fields that may have various effects anywhere along the request-response chain.
        /// <example><code>Pragma: no-cache</code></example>
        /// </summary>
        public const string Pragma = "Pragma";

        /// <summary>
        /// Request authentication to access the proxy.
        /// <example><code>Proxy-Authenticate: Basic</code></example>
        /// </summary>
        public const string ProxyAuthenticate = "Proxy-Authenticate";

        /// <summary>
        /// <c>HTTP</c> Public Key Pinning, announces hash of website's authentic <c>TLS</c> certificate.
        /// <example><code>Public-Key-Pins: max-age=2592000; pin-sha256="E9CZ9INDbd+2eRQozYqqbQ2yXLVKB9+xcprMF+44U1g=";</code></example>
        /// </summary>
        public const string PublicKeyPins = "Public-Key-Pins";

        /// <summary>
        /// Used in redirection, or when a new resource has been created. This refresh redirects after 5 seconds.
        /// <example><code>Refresh: 5; url=http://www.w3.org/pub/WWW/People.html</code></example>
        /// </summary>
        public const string Refresh = "Refresh";

        /// <summary>
        /// If an entity is temporarily unavailable, this instructs the client to try again later. 
        /// Value could be a specified period of time (in seconds) or a <c>HTTP-date</c>.
        /// <example><code>Retry-After: 120</code></example>
        /// <example><code>Retry-After: Fri, 07 Nov 2014 23:59:59 GMT</code></example>
        /// </summary>
        public const string RetryAfter = "Retry-After";

        /// <summary>
        /// A name for the server.
        /// <example><code>Server: Apache/2.4.1 (Unix)</code></example>
        /// </summary>
        public const string Server = "Server";

        /// <summary>
        /// An HTTP cookie.
        /// <example><code>Set-Cookie: UserID=JohnDoe; Max-Age=3600; Version=1</code></example>
        /// </summary>
        public const string SetCookie = "Set-Cookie";

        /// <summary>
        /// <c>CGI</c> header field specifying the status of the <c>HTTP</c> response. 
        /// Normal <c>HTTP</c> responses use a separate <c>"Status-Line"</c> instead, defined by <c>RFC 7230</c>.
        /// <example><code>Status: 200 OK</code></example>
        /// </summary>
        public const string Status = "Status";

        /// <summary>
        /// A HSTS Policy informing the HTTP client how long to cache the HTTPS only policy and 
        /// whether this applies to subdomains.
        /// <example><code>Strict-Transport-Security: max-age=16070400; includeSubDomains</code></example>
        /// </summary>
        public const string StrictTransportSecurity = "Strict-Transport-Security";

        /// <summary>
        /// The form of encoding used to safely transfer the entity to the user.
        /// <remarks>
        /// Currently defined methods are: <c>chunked</c>, <c>compress</c>, <c>deflate</c>, <c>gzip</c>, <c>identity</c>.
        /// </remarks>
        /// <example><code>Transfer-Encoding: chunked</code></example>
        /// </summary>
        public const string TransferEncoding = "Transfer-Encoding";

        /// <summary>
        /// Ask the client to upgrade to another protocol.
        /// <example><code>Upgrade: HTTP/2.0, HTTPS/1.3, IRC/6.9, RTA/x11, websocket</code></example>
        /// </summary>
        public const string Upgrade = "Upgrade";

        /// <summary>
        /// Tells downstream proxies how to match future request headers to decide whether 
        /// the cached response can be used rather than requesting a fresh one from the origin server.
        /// <example><code>Vary: *</code></example>
        /// <example><code>Vary: Accept-Language</code></example>
        /// </summary>
        public const string Vary = "Vary";

        /// <summary>
        /// Informs the client of proxies through which the response was sent.
        /// <example><code>Via: 1.0 fred, 1.1 example.com (Apache/1.1)</code></example>
        /// </summary>
        public const string Via = "Via";

        /// <summary>
        /// Indicates the authentication scheme that should be used to access the requested entity.
        /// <example><code>WWW-Authenticate: Basic</code></example>
        /// </summary>
        public const string WWWAuthenticate = "WWW-Authenticate";
    }
}