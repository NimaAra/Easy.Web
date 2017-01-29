namespace Easy.Web.Tests.Unit.HelpersTests
{
    using Easy.Web.Core.Helpers;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    internal sealed class HttpRequestHeadersTests
    {
        [Test]
        public void Run()
        {
            HttpRequestHeaders.Accept.ShouldBe("Accept");
            HttpRequestHeaders.AcceptCharset.ShouldBe("Accept-Charset");
            HttpRequestHeaders.AcceptEncoding.ShouldBe("Accept-Encoding");
            HttpRequestHeaders.AcceptLanguage.ShouldBe("Accept-Language");
            HttpRequestHeaders.Authorization.ShouldBe("Authorization");
            HttpRequestHeaders.CacheControl.ShouldBe("Cache-Control");
            HttpRequestHeaders.Cookie.ShouldBe("Cookie");
            HttpRequestHeaders.Connection.ShouldBe("Connection");
            HttpRequestHeaders.ContentLength.ShouldBe("Content-Length");
            HttpRequestHeaders.ContentType.ShouldBe("Content-Type");
            HttpRequestHeaders.Date.ShouldBe("Date");
            HttpRequestHeaders.Referer.ShouldBe("Referer");
            HttpRequestHeaders.ProxyAuthorization.ShouldBe("Proxy-Authorization");
            HttpRequestHeaders.UserAgent.ShouldBe("User-Agent");
            HttpRequestHeaders.Origin.ShouldBe("Origin");
            HttpRequestHeaders.Host.ShouldBe("Host");
            HttpRequestHeaders.From.ShouldBe("From");
            HttpRequestHeaders.Forwarded.ShouldBe("Forwarded");
            HttpRequestHeaders.Pragma.ShouldBe("Pragma");
            HttpRequestHeaders.Upgrade.ShouldBe("Upgrade");
            HttpRequestHeaders.Via.ShouldBe("Via");
            HttpRequestHeaders.IfMatch.ShouldBe("If-Match");
            HttpRequestHeaders.IfModifiedSince.ShouldBe("If-Modified-Since");
            HttpRequestHeaders.IfNoneMatch.ShouldBe("If-None-Match");
            HttpRequestHeaders.IfRange.ShouldBe("If-Range");
            HttpRequestHeaders.IfUnmodifiedSince.ShouldBe("If-Unmodified-Since");
            HttpRequestHeaders.MaxForwards.ShouldBe("Max-Forwards");
            HttpRequestHeaders.Warning.ShouldBe("Warning");
        }
    }
}