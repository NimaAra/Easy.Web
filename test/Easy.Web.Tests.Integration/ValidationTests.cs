namespace Easy.Web.Tests.Integration
{
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Easy.Web.Core.Helpers;
    using Shouldly;
    using HttpStatusCode = System.Net.HttpStatusCode;

    internal sealed class ValidationTests
    {
        private static HttpClient _client;

        internal static async Task Run(HttpClient client)
        {
            _client = client;

            await When_posting_valid_json_content_from_body();
            await When_posting_valid_json_partial_content_from_body();
            await When_posting_invalid_json_content_from_body();
            await When_posting_invalid_json_partial_content_from_body();
        }

        private static async Task When_posting_valid_json_content_from_body()
        {
            const string JsonContent = "{Id:1, Category: \"Valid\"}";

            var content = new StringContent(JsonContent, Encoding.UTF8, MediaTypes.JSON);
            var resp = await _client.PostAsync("validate/body/valid", content);
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_posting_valid_json_partial_content_from_body()
        {
            const string JsonContent = "{Id:1, Category: \"Valid\"}";

            var content = new StringContent(JsonContent, Encoding.UTF8, MediaTypes.JSON);
            var resp = await _client.PostAsync("validate/body/validPartial", content);
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_posting_invalid_json_content_from_body()
        {
            const string JsonContent = "{ Category: \"Invalid Category\" }";

            var content = new StringContent(JsonContent, Encoding.UTF8, MediaTypes.JSON);
            var resp = await _client.PostAsync("validate/body/invalid", content);
            resp.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("139");

            headers.ShouldContain(h => h.Key == "Content-Type");
            headers[1].Value.First().ShouldBe(MediaTypes.JSON);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("[{\"memberNames\":[\"Id\"],\"errorMessage\":\"The Id field is required.\"},{\"memberNames\":[\"Category\"],\"errorMessage\":\"Maximum length failed.\"}]");
        }

        private static async Task When_posting_invalid_json_partial_content_from_body()
        {
            const string JsonContent = "{Id:1, Category: \"Valid\"}";

            var content = new StringContent(JsonContent, Encoding.UTF8, MediaTypes.JSON);
            var resp = await _client.PostAsync("validate/body/invalidPartial", content);
            resp.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("70");

            headers.ShouldContain(h => h.Key == "Content-Type");
            headers[1].Value.First().ShouldBe(MediaTypes.JSON);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("[{\"memberNames\":[\"Id\"],\"errorMessage\":\"The Id field is required.\"}]");
        }
    }
}