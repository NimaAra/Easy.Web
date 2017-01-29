namespace Easy.Web.Tests.Integration
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Easy.Web.Core.Helpers;
    using Shouldly;
    using HttpStatusCode = System.Net.HttpStatusCode;

    internal sealed class SerializeTests
    {
        private static HttpClient _client;

        internal static async Task Run(HttpClient client)
        {
            _client = client;

            await When_getting_reply_as_serialzed_json();
            await When_getting_reply_as_serialzed_xml();
            await When_getting_reply_as_serialzed_binary();
        }

        private static async Task When_getting_reply_as_serialzed_json()
        {
            var resp = await _client.GetAsync("serialize/json");
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

        private static async Task When_getting_reply_as_serialzed_xml()
        {
            var resp = await _client.GetAsync("serialize/xml");
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

        private static async Task When_getting_reply_as_serialzed_binary()
        {
            var resp = await _client.GetAsync("serialize/binary");
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
    }
}