namespace Easy.Web.Tests.Integration
{
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Easy.Web.Core.Helpers;
    using Shouldly;
    using HttpRequestHeaders = Easy.Web.Core.Helpers.HttpRequestHeaders;
    using HttpStatusCode = System.Net.HttpStatusCode;

    internal sealed class ContentNegotiationTests
    {
        private static HttpClient _client;

        internal static async Task Run(HttpClient client)
        {
            _client = client;

            await When_getting_negotiated_reply_with_no_accept_header();
            await When_getting_negotiated_reply_with_json_accept_header();
            await When_getting_negotiated_reply_with_xml_accept_header();
            await When_getting_negotiated_reply_with_binary_accept_header();
            await When_getting_negotiated_reply_with_not_supported_accept_header();
            await When_getting_negotiated_reply_with_multiple_accept_headers_and_only_one_supported();
            await When_getting_negotiated_reply_with_multiple_accept_headers_and_all_supported();
        }

        private static async Task When_getting_negotiated_reply_with_no_accept_header()
        {
            var resp = await _client.GetAsync("negotiate");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("40");
            headers[1].Value.First().ShouldBe(MediaTypes.JSON);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("{\"id\":123,\"category\":\"Some category\"}");
        }

        private static async Task When_getting_negotiated_reply_with_not_supported_accept_header()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("foo/bar"));
            var resp = await _client.GetAsync("negotiate");

            _client.DefaultRequestHeaders.Accept.Clear();

            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("40");
            headers[1].Value.First().ShouldBe(MediaTypes.JSON);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("{\"id\":123,\"category\":\"Some category\"}");
        }

        private static async Task When_getting_negotiated_reply_with_json_accept_header()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.JSON));
            var resp = await _client.GetAsync("negotiate");

            _client.DefaultRequestHeaders.Accept.Clear();

            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("40");
            headers[1].Value.First().ShouldBe(MediaTypes.JSON);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("{\"id\":123,\"category\":\"Some category\"}");
        }

        private static async Task When_getting_negotiated_reply_with_xml_accept_header()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.XML));
            var resp = await _client.GetAsync("negotiate");

            _client.DefaultRequestHeaders.Accept.Clear();

            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("225");
            headers[1].Value.First().ShouldBe(MediaTypes.XML);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe(@"<?xml version=""1.0"" encoding=""utf-8""?>
<SampleModel xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>123</Id>
  <Category>Some category</Category>
</SampleModel>");
        }

        private static async Task When_getting_negotiated_reply_with_binary_accept_header()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.Binary));
            var resp = await _client.GetAsync("negotiate");

            _client.DefaultRequestHeaders.Accept.Clear();

            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("17");
            headers[1].Value.First().ShouldBe(MediaTypes.Binary);

            var resBytes = await resp.Content.ReadAsByteArrayAsync();
            resBytes.ShouldNotBeNull();
            resBytes.Length.ShouldBe(17);
        }

        private static async Task When_getting_negotiated_reply_with_multiple_accept_headers_and_only_one_supported()
        {
            _client.DefaultRequestHeaders.Add(HttpRequestHeaders.Accept, new [] { "foo/bar", "text/xml" });
            var resp = await _client.GetAsync("negotiate");

            _client.DefaultRequestHeaders.Accept.Clear();

            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("225");
            headers[1].Value.First().ShouldBe("text/xml");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe(@"<?xml version=""1.0"" encoding=""utf-8""?>
<SampleModel xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>123</Id>
  <Category>Some category</Category>
</SampleModel>");
        }

        private static async Task When_getting_negotiated_reply_with_multiple_accept_headers_and_all_supported()
        {
            _client.DefaultRequestHeaders.Add(HttpRequestHeaders.Accept, new[] { "application/xml; charset=utf-8", "application/json" });
            var resp = await _client.GetAsync("negotiate");

            _client.DefaultRequestHeaders.Accept.Clear();

            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("225");
            headers[1].Value.First().ShouldBe(MediaTypes.XML);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe(@"<?xml version=""1.0"" encoding=""utf-8""?>
<SampleModel xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>123</Id>
  <Category>Some category</Category>
</SampleModel>");
        }
    }
}