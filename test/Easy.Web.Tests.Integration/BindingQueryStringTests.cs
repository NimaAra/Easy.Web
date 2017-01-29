namespace Easy.Web.Tests.Integration
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Easy.Web.Core.Helpers;
    using Shouldly;
    using HttpStatusCode = System.Net.HttpStatusCode;

    internal sealed class BindingQueryStringTests
    {
        private static HttpClient _client;

        internal static async Task Run(HttpClient client)
        {
            _client = client;

            await When_binding_to_empty_query_string_as_dynamic_dictionary();
            await When_binding_to_query_string_as_dynamic_dictionary();
            //await When_binding_to_query_string_as_dynamic(); //[ToDo] - Ignore for now
            await When_binding_to_query_string_as_model();
            await When_binding_to_query_string_as_model_with_selective_property_names();
        }

        private static async Task When_binding_to_empty_query_string_as_dynamic_dictionary()
        {
            var resp = await _client.GetAsync("bind/noQueryString");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_binding_to_query_string_as_dynamic_dictionary()
        {
            var resp = await _client.GetAsync("bind/queryString?id=10&category=some+Category");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("32");
            headers[1].Value.First().ShouldBe(MediaTypes.TEXT);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("Id: 10 - Category: some Category");
        }

        private static async Task When_binding_to_query_string_as_dynamic()
        {
            var resp = await _client.GetAsync("bind/queryStringDynamic?id=10&category=some+Category");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("32");
            headers[1].Value.First().ShouldBe(MediaTypes.TEXT);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("Id: 10 - Category: some Category");
        }

        private static async Task When_binding_to_query_string_as_model()
        {
            var resp = await _client.GetAsync("bind/queryStringModel?id=10&category=some+Category");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("32");
            headers[1].Value.First().ShouldBe(MediaTypes.TEXT);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("Id: 10 - Category: some Category");
        }

        private static async Task When_binding_to_query_string_as_model_with_selective_property_names()
        {
            var resp = await _client.GetAsync("bind/queryStringModelSelective?id=10&category=some+Category");
            resp.EnsureSuccessStatusCode();

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(2);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers.ShouldContain(h => h.Key == "Content-Type");

            headers[0].Value.First().ShouldBe("23");
            headers[1].Value.First().ShouldBe(MediaTypes.TEXT);

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBe("Category: some Category");
        }
    }
}