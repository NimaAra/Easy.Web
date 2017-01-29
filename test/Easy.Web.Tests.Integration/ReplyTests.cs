namespace Easy.Web.Tests.Integration
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Easy.Web.Core.Helpers;
    using Shouldly;
    using HttpStatusCode = System.Net.HttpStatusCode;

    internal sealed class ReplyTests
    {
        private static HttpClient _client;

        internal static async Task Run(HttpClient client)
        {
            _client = client;

            await When_getting_reply_from_non_existent_endpoint();
            await When_getting_reply_as_status_code();
            await When_getting_reply_as_custom();
            await When_getting_reply_as_text_raw();
            await When_getting_reply_as_text();
            await When_getting_reply_as_html();
            await When_getting_reply_as_json();
            await When_getting_reply_as_xml();
            await When_getting_reply_as_binary_array();
            await When_getting_reply_as_binary_stream();
            await When_getting_reply_as_file();
        }

        private static async Task When_getting_reply_from_non_existent_endpoint()
        {
            var resp = await _client.GetAsync("reply/nonExistent");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.ShouldBeEmpty();

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("You will only see this message if no handler handled the request.");
        }

        private static async Task When_getting_reply_as_status_code()
        {
            var resp = await _client.GetAsync("reply/statusCode");

            resp.StatusCode.ShouldBe(HttpStatusCode.BadGateway);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_getting_reply_as_custom()
        {
            var resp = await _client.GetAsync("reply/custom");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.Created);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("14");
            headers[1].Value.First().ShouldBe("foo/bar");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("This is custom");
        }

        private static async Task When_getting_reply_as_text_raw()
        {
            var resp = await _client.GetAsync("reply/raw");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(0);
            
            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("This is raw text");
        }

        private static async Task When_getting_reply_as_text()
        {
            var resp = await _client.GetAsync("reply/text");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("17");
            headers[1].Value.First().ShouldBe(MediaTypes.TEXT);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("This is some text");
        }

        private static async Task When_getting_reply_as_html()
        {
            var resp = await _client.GetAsync("reply/html");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("16");
            headers[1].Value.First().ShouldBe(MediaTypes.HTML);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("<p>Say what!</p>");
        }

        private static async Task When_getting_reply_as_json()
        {
            var resp = await _client.GetAsync("reply/json");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("10");
            headers[1].Value.First().ShouldBe(MediaTypes.JSON);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("{name:foo}");
        }

        private static async Task When_getting_reply_as_xml()
        {
            var resp = await _client.GetAsync("reply/xml");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("22");
            headers[1].Value.First().ShouldBe(MediaTypes.XML);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("<element>foo</element>");
        }

        private static async Task When_getting_reply_as_binary_array()
        {
            var resp = await _client.GetAsync("reply/binaryArray");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("3");
            headers[1].Value.First().ShouldBe(MediaTypes.Binary);

            var resBytes = await resp.Content.ReadAsByteArrayAsync();
            resBytes.ShouldBe(new byte[] {1, 0, 1});
        }

        private static async Task When_getting_reply_as_binary_stream()
        {
            var resp = await _client.GetAsync("reply/binaryStream");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("3");
            headers[1].Value.First().ShouldBe(MediaTypes.Binary);

            using (var resStream = await resp.Content.ReadAsStreamAsync())
            using (var memStream = new MemoryStream())
            {
                resStream.CopyTo(memStream);
                memStream.ToArray().ShouldBe(new byte []{0, 1, 0});
            }
        }

        private static async Task When_getting_reply_as_file()
        {
            var resp = await _client.GetAsync("reply/file");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(3);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");
            headers.ShouldContain(h => h.Key == "Content-Disposition");

            headers[0].Value.First().ShouldBe("13");
            headers[1].Value.First().ShouldBe("attachment; filename=Sample.txt");
            headers[2].Value.First().ShouldBe(MediaTypes.TEXT);

            using (var resStream = await resp.Content.ReadAsStreamAsync())
            using (var memStream = new MemoryStream())
            {
                resStream.CopyTo(memStream);
                memStream.ToArray()
                    .ShouldBe(File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample.txt")));
            }
        }
    }
}