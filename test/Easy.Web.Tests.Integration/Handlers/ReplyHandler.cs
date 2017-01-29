namespace Easy.Web.Tests.Integration.Handlers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Easy.Web.Core.Models;
    using Microsoft.AspNetCore.Http;
    using Core.Routing;
    using Easy.Web.Core.Extensions;
    using Easy.Web.Core.Helpers;

    internal sealed class ReplyHandler : Handler
    {
        [Route(HttpMethod.GET, "reply/statusCode")]
        public Task ReplyWithStatus(HttpContext context)
        {
            return context.ReplyAsStatus(HttpStatusCode.BadGateway);
        }

        [Route(HttpMethod.GET, "reply/custom")]
        public Task ReplyCustom(HttpContext context)
        {
            return context.Reply("This is custom", "foo/bar", HttpStatusCode.Created);
        }

        [Route(HttpMethod.GET, "reply/raw")]
        public Task ReplyTextRaw(HttpContext context)
        {
            return context.Response.WriteAsync("This is raw text");
        }

        [Route(HttpMethod.GET, "reply/text")]
        public Task ReplyText(HttpContext context)
        {
            return context.ReplyAsText("This is some text");
        }

        [Route(HttpMethod.GET, "reply/html")]
        public Task ReplyHtml(HttpContext context)
        {
            return context.ReplyAsHTML("<p>Say what!</p>");
        }

        [Route(HttpMethod.GET, "reply/json")]
        public Task ReplyJson(HttpContext context)
        {
            return context.ReplyAsJSON("{name:foo}");
        }

        [Route(HttpMethod.GET, "reply/xml")]
        public Task ReplyXml(HttpContext context)
        {
            return context.ReplyAsXML("<element>foo</element>");
        }

        [Route(HttpMethod.GET, "reply/binaryArray")]
        public Task ReplyBinaryArray(HttpContext context)
        {
            return context.ReplyAsBinary(new byte[] {1, 0, 1}, MediaTypes.Binary);
        }

        [Route(HttpMethod.GET, "reply/binaryStream")]
        public Task ReplyBinaryStream(HttpContext context)
        {
            using (var memStream = new MemoryStream(new byte[] { 0, 1, 0 }))
            {
                return context.ReplyAsStream(memStream, MediaTypes.Binary);
            }
        }

        [Route(HttpMethod.GET, "reply/file")]
        public Task ReplyFile(HttpContext context)
        {
            var file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample.txt"));
            return context.ReplyAsFile(file, MediaTypes.TEXT);
        }
    }
}