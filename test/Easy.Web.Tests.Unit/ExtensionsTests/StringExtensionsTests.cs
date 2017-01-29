namespace Easy.Web.Tests.Unit.ExtensionsTests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Easy.Web.Core.Extensions;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal sealed class StringExtensionsTests
    {
        [TestCase("application/x-www-form-urlencoded", true)]
        [TestCase("application/x-www-FORM-urlencoded", true)]
        [TestCase("application/x-www-form-urlencoded; charset=utf-8", true)]
        [TestCase("application/x-www-form-urlencoded;charset=utf-8", true)]
        [TestCase("application/x-www-form-urlencoded;charset=utf-8", true)]
        [TestCase("application/x-www-form-urlencoded; charset= utf-8", true)]
        [TestCase("application/x-www-form-urlencoded; charset =utf-8", true)]
        [TestCase("application/x-www-form-urlencoded; charset = utf-8", true)]
        [TestCase("application/x-www-form-urlencoded charset=utf-8", true)]

        [TestCase("application/json", false)]
        [TestCase("application/x-www-form-rulencoded charset=utf-8", false)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("foo", false)]
        public void When_checking_form_url_encoded_media_type(string input, bool expectedResult)
        {
            input.IsFormUrlEncodedMediaType().ShouldBe(expectedResult);
        }

        [TestCase("application/json", true)]
        [TestCase("application/json;", true)]
        [TestCase("application/JSON", true)]
        [TestCase("application/JSON;", true)]
        [TestCase("text/JSON", true)]
        [TestCase("text/JSON;", true)]
        [TestCase("tEXt/json", true)]
        [TestCase("application/vnd.api+json", true)]
        [TestCase("application/vnd.foo+json", true)]
        [TestCase("application/json; charset=utf-8", true)]
        [TestCase("application/json;charset=utf-8", true)]
        [TestCase("text/json; charset= utf-8", true)]
        [TestCase("text/json;charset= utf-8", true)]
        [TestCase("application/json; charset =utf-8", true)]
        [TestCase("text/json; charset = utf-8", true)]
        [TestCase("application/json charset=utf-8", true)]
        [TestCase("application/vnd.+json charset=utf-8", true)]

        [TestCase("application/xml", false)]
        [TestCase("application/x-www-form-rulencoded charset=utf-8", false)]
        [TestCase("json", false)]
        [TestCase("application/vnd", false)]
        [TestCase("application/vnd+json", false)]
        [TestCase("application/vndfoo+json", false)]
        [TestCase("application/vnd.json", false)]
        [TestCase("application/vnd/json", false)]
        [TestCase("+json", false)]
        [TestCase("application/+json", false)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("foo", false)]
        public void When_checking_JSON_media_type(string input, bool expectedResult)
        {
            input.IsJSONMediaType().ShouldBe(expectedResult);
        }

        [TestCase("application/xml", true)]
        [TestCase("application/xml;", true)]
        [TestCase("application/XML", true)]
        [TestCase("application/XML;", true)]
        [TestCase("text/XML", true)]
        [TestCase("text/XML;", true)]
        [TestCase("tEXt/xml", true)]
        [TestCase("application/vnd.api+xml", true)]
        [TestCase("application/vnd.foo+xml", true)]
        [TestCase("application/xml; charset=utf-8", true)]
        [TestCase("application/xml;charset=utf-8", true)]
        [TestCase("text/xml; charset= utf-8", true)]
        [TestCase("text/xml;charset= utf-8", true)]
        [TestCase("application/xml; charset =utf-8", true)]
        [TestCase("text/xml; charset = utf-8", true)]
        [TestCase("application/xml charset=utf-8", true)]
        [TestCase("application/vnd.+xml charset=utf-8", true)]

        [TestCase("application/json", false)]
        [TestCase("application/x-www-form-rulencoded charset=utf-8", false)]
        [TestCase("xml", false)]
        [TestCase("application/vnd", false)]
        [TestCase("application/vnd+xml", false)]
        [TestCase("application/vndfoo+xml", false)]
        [TestCase("application/vnd.xml", false)]
        [TestCase("application/vnd/xml", false)]
        [TestCase("+xml", false)]
        [TestCase("application/+xml", false)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("foo", false)]
        public void When_checking_XML_media_type(string input, bool expectedResult)
        {
            input.IsXMLMediaType().ShouldBe(expectedResult);
        }

        [TestCase("application/octet-stream", true)]
        [TestCase("application/OCTEt-stream", true)]
        [TestCase("application/octet-stream; charset=utf-8", true)]
        [TestCase("application/octet-stream;charset=utf-8", true)]
        [TestCase("application/octet-stream;charset=utf-8", true)]
        [TestCase("application/octet-stream; charset= utf-8", true)]
        [TestCase("application/octet-stream; charset =utf-8", true)]
        [TestCase("application/octet-stream; charset = utf-8", true)]
        [TestCase("application/octet-stream charset=utf-8", true)]

        [TestCase("application/json", false)]
        [TestCase("application/x-www-form-rulencoded charset=utf-8", false)]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("foo", false)]
        public void When_checking_binary_media_type(string input, bool expectedResult)
        {
            input.IsBinaryMediaType().ShouldBe(expectedResult);
        }
        
        [Test]
        public void When_extracting_media_types_from_header()
        {
            var mediaTypes = string.Empty.ExtractMediaTypes().ToArray();
            mediaTypes.Length.ShouldBe(1);
            mediaTypes[0].ShouldBe(string.Empty);

            mediaTypes = "Foo".ExtractMediaTypes().ToArray();
            mediaTypes.Length.ShouldBe(1);
            mediaTypes[0].ShouldBe("Foo");

            mediaTypes = "application/json, application/xml".ExtractMediaTypes().ToArray();
            mediaTypes.Length.ShouldBe(2);
            mediaTypes[0].ShouldBe("application/json");
            mediaTypes[1].ShouldBe("application/xml");

            mediaTypes = "application/json, application/xml;".ExtractMediaTypes().ToArray();
            mediaTypes.Length.ShouldBe(2);
            mediaTypes[0].ShouldBe("application/json");
            mediaTypes[1].ShouldBe("application/xml");

            mediaTypes = "application/json, application/xml; ".ExtractMediaTypes().ToArray();
            mediaTypes.Length.ShouldBe(2);
            mediaTypes[0].ShouldBe("application/json");
            mediaTypes[1].ShouldBe("application/xml");

            mediaTypes = "application/json, application/xml; charset=utf-8".ExtractMediaTypes().ToArray();
            mediaTypes.Length.ShouldBe(2);
            mediaTypes[0].ShouldBe("application/json");
            mediaTypes[1].ShouldBe("application/xml");
        }

        [TestCase("application/json;charset=foo", "foo")]
        [TestCase("charset=utf-8", "utf-8")]
        [TestCase("application/json;charset=utf-8", "utf-8")]
        [TestCase("application/json;charset =utf-8", "utf-8")]
        [TestCase("application/json;charset = utf-8", "utf-8")]
        [TestCase("application/json;charset= utf-8", "utf-8")]
        [TestCase("application/json; charset= utf-8", "utf-8")]
        [TestCase("application/json; charset= utf-8;", "utf-8")]
        public void When_getting_character_set_success(string input, string characterSet)
        {
            string result;
            input.TryExtractCharacterSet(out result).ShouldBeTrue();
            result.ShouldBe(characterSet);
        }

        [TestCase("application/json;charsetfoo")]
        [TestCase("application/json;charset-utf-8")]
        [TestCase("application/json;charset=utf-8;;")]
        [TestCase("application/json;charset=  utf-8")]
        [TestCase("application/json;charset  =  utf-8")]
        [TestCase("application/json;charset  =utf-8")]
        [TestCase("application/json;charset=utf -8")]
        [TestCase("application/json; charset=;")]
        [TestCase("application/json; charset=;;")]
        public void When_getting_character_set_failure(string input)
        {
            string result;
            input.TryExtractCharacterSet(out result).ShouldBeFalse();
            result.ShouldBeNull();
        }
    }
}