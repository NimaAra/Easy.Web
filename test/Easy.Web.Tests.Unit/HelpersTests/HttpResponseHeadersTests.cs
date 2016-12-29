namespace Easy.Web.Tests.Unit.HelpersTests
{
    using Core.Models;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    internal sealed class HttpResponseHeadersTests
    {
        [Test]
        public void Run()
        {
            HttpResponseHeaders.AccessControlAllowOrigin.ShouldBe("Access-Control-Allow-Origin");
            HttpResponseHeaders.AcceptRanges.ShouldBe("Accept-Ranges");
            HttpResponseHeaders.Age.ShouldBe("Age");
            HttpResponseHeaders.Date.ShouldBe("Date");
            HttpResponseHeaders.ETag.ShouldBe("ETag");
            HttpResponseHeaders.Expires.ShouldBe("Expires");
            HttpResponseHeaders.LastModified.ShouldBe("Last-Modified");
            HttpResponseHeaders.Link.ShouldBe("Link");
            HttpResponseHeaders.Allow.ShouldBe("Allow");
            HttpResponseHeaders.CacheControl.ShouldBe("Cache-Control");
            HttpResponseHeaders.Connection.ShouldBe("Connection");
            HttpResponseHeaders.ContentDisposition.ShouldBe("Content-Disposition");
            HttpResponseHeaders.ContentEncoding.ShouldBe("Content-Encoding");
            HttpResponseHeaders.ContentLanguage.ShouldBe("Content-Language");
            HttpResponseHeaders.ContentLength.ShouldBe("Content-Length");
            HttpResponseHeaders.ContentLocation.ShouldBe("Content-Location");
            HttpResponseHeaders.ContentMD5.ShouldBe("Content-MD5");
            HttpResponseHeaders.ContentRange.ShouldBe("Content-Range");
            HttpResponseHeaders.ContentType.ShouldBe("Content-Type");
            HttpResponseHeaders.Location.ShouldBe("Location");
            HttpResponseHeaders.Pragma.ShouldBe("Pragma");
            HttpResponseHeaders.ProxyAuthenticate.ShouldBe("Proxy-Authenticate");
            HttpResponseHeaders.PublicKeyPins.ShouldBe("Public-Key-Pins");
            HttpResponseHeaders.Refresh.ShouldBe("Refresh");
            HttpResponseHeaders.RetryAfter.ShouldBe("Retry-After");
            HttpResponseHeaders.Server.ShouldBe("Server");
            HttpResponseHeaders.SetCookie.ShouldBe("Set-Cookie");
            HttpResponseHeaders.Status.ShouldBe("Status");
            HttpResponseHeaders.StrictTransportSecurity.ShouldBe("Strict-Transport-Security");
            HttpResponseHeaders.TransferEncoding.ShouldBe("Transfer-Encoding");
            HttpResponseHeaders.Upgrade.ShouldBe("Upgrade");
            HttpResponseHeaders.Vary.ShouldBe("Vary");
            HttpResponseHeaders.Via.ShouldBe("Via");
            HttpResponseHeaders.WWWAuthenticate.ShouldBe("WWW-Authenticate");
        }
    }
}