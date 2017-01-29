namespace Easy.Web.Tests.Unit.HelpersTests
{
    using System.Text;
    using Easy.Web.Core.Helpers;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    internal sealed class EncodingsTests
    {
        [TestCase("utf-8", "Unicode (UTF-8)")]
        [TestCase("UTF-8", "Unicode (UTF-8)")]
        [TestCase("UTF-7", "Unicode (UTF-7)")]
        [TestCase("iso-8859-15", "Latin 9 (ISO)")]
        [TestCase("IBM037", "IBM EBCDIC (US-Canada)")]
        [TestCase("iso-8859-4", "Baltic (ISO)")]
        [TestCase("windows-1257", "Baltic (Windows)")]
        public void When_getting_valid_encoding(string charset, string encodingName)
        {
            Encoding encoding;
            Encodings.TryGetEncoding(charset, out encoding).ShouldBeTrue();
            encoding.ShouldNotBeNull();
            encoding.WebName.ShouldBe(charset, StringCompareShould.IgnoreCase);
            encoding.EncodingName.ShouldBe(encodingName);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("foo")]
        [TestCase("utf8")]
        [TestCase("utf-88")]
        public void When_getting_invalid_encoding(string charset)
        {
            Encoding ignored;
            Encodings.TryGetEncoding(charset, out ignored).ShouldBeFalse();
            ignored.ShouldBeNull();
        }
    }
}