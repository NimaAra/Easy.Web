namespace Easy.Web.Tests.Unit.HelpersTests
{
    using Core.Models;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    internal sealed class MediaTypesTests
    {
        [Test]
        public void Run()
        {
            MediaTypes.TEXT.ShouldBe("text/plain");
            MediaTypes.JSON.ShouldBe("application/json");
            MediaTypes.XML.ShouldBe("text/xml");
            MediaTypes.HTML.ShouldBe("text/html");
            MediaTypes.CSS.ShouldBe("text/css");
            MediaTypes.JavaScript.ShouldBe("application/javascript");
            MediaTypes.PDF.ShouldBe("application/pdf");
            MediaTypes.Binary.ShouldBe("application/octet-stream");
            MediaTypes.Form.ShouldBe("multipart/form-data");
            MediaTypes.FormUrlEncoded.ShouldBe("application/x-www-form-urlencoded");
            MediaTypes.ZIP.ShouldBe("application/zip");
            MediaTypes.GIF.ShouldBe("image/gif");
            MediaTypes.JPEG.ShouldBe("image/jpeg");
            MediaTypes.PNG.ShouldBe("image/png");
        }
    }
}