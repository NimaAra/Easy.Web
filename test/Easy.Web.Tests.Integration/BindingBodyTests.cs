namespace Easy.Web.Tests.Integration
{
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Easy.Web.Core.Helpers;
    using Easy.Web.Tests.Integration.Models;
    using Shouldly;
    using HttpStatusCode = System.Net.HttpStatusCode;

    internal sealed class BindingBodyTests
    {
        private static HttpClient _client;

        internal static async Task Run(HttpClient client)
        {
            _client = client;

            await When_binding_to_empty_body_as_dynamic_dictionary();
            await When_binding_get_with_no_content_to_body();
            await When_binding_form_url_encoded_content_to_body();
            await When_binding_xml_content_to_body();
            await When_binding_json_content_to_body();
            await When_binding_binary_content_to_body();
            await When_binding_xml_content_to_body_with_partial_content_allowed();
            await When_binding_json_content_to_body_with_partial_content_allowed();
            await When_binding_binary_content_to_body_with_partial_content_allowed();
            await When_binding_json_content_to_body_as_put();
            await When_binding_json_content_to_body_which_is_not_what_the_server_is_expecting();
        }

        private static async Task When_binding_to_empty_body_as_dynamic_dictionary()
        {
            var resp = await _client.GetAsync("bind/bodyNoContentType");

            resp.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_binding_get_with_no_content_to_body()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.FormUrlEncoded));
            var resp = await _client.GetAsync("bind/bodyNoContentType");

            _client.DefaultRequestHeaders.Clear();

            resp.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_binding_form_url_encoded_content_to_body()
        {
            var content = new StringContent("category=some+Category", Encoding.UTF8, MediaTypes.FormUrlEncoded);
            var resp = await _client.PostAsync("bind/bodyHasContent", content);
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_binding_xml_content_to_body()
        {
            const string XmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<SampleModel xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>123</Id>
  <Category>Some category</Category>
</SampleModel>";

            var content = new StringContent(XmlContent, Encoding.UTF8, MediaTypes.XML);
            var resp = await _client.PostAsync("bind/bodyHasContent", content);
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_binding_json_content_to_body()
        {
            const string JsonContent = "{Id:123, Category: \"Some category\"}";

            var content = new StringContent(JsonContent, Encoding.UTF8, MediaTypes.JSON);
            var resp = await _client.PostAsync("bind/bodyHasContent", content);
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_binding_binary_content_to_body()
        {
            byte[] binaryContent;
            using (var mem = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(mem, new SampleModel {Id = 123, Category = "Some category"});
                binaryContent = mem.ToArray();
            }
            
            var content = new ByteArrayContent(binaryContent);
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypes.Binary);
            var resp = await _client.PostAsync("bind/bodyHasContent", content);
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_binding_xml_content_to_body_with_partial_content_allowed()
        {
            const string XmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<SampleModel xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>123</Id>
  <Category>some Category</Category>
</SampleModel>";

            var content = new StringContent(XmlContent, Encoding.UTF8, MediaTypes.XML);
            var resp = await _client.PostAsync("bind/bodyHasContentSelective", content);
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_binding_json_content_to_body_with_partial_content_allowed()
        {
            const string JsonContent = "{Id:123, Category: \"some Category\"}";

            var content = new StringContent(JsonContent, Encoding.UTF8, MediaTypes.JSON);
            var resp = await _client.PostAsync("bind/bodyHasContentSelective", content);
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_binding_binary_content_to_body_with_partial_content_allowed()
        {
            byte[] binaryContent;
            using (var mem = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(mem, new SampleModel { Id = 123, Category = "some Category" });
                binaryContent = mem.ToArray();
            }

            var content = new ByteArrayContent(binaryContent);
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypes.Binary);
            var resp = await _client.PostAsync("bind/bodyHasContentSelective", content);
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_binding_json_content_to_body_as_put()
        {
            const string JsonContent = "{Id:123, Category: \"some Category\"}";

            var content = new StringContent(JsonContent, Encoding.UTF8, MediaTypes.JSON);
            var resp = await _client.PutAsync("bind/putting", content);
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_binding_json_content_to_body_which_is_not_what_the_server_is_expecting()
        {
            const string JsonContent = "{Name:\"Foo\", User: \"some User\"}";

            var content = new StringContent(JsonContent, Encoding.UTF8, MediaTypes.JSON);
            var resp = await _client.PostAsync("bind/bodyHasInvalidContent", content);
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }
    }
}