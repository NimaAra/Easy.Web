namespace Easy.Web.Tests.Integration
{
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Shouldly;

    internal sealed class RouteDataTests
    {
        private static HttpClient _client;

        internal static async Task Run(HttpClient client)
        {
            _client = client;

            await When_obtaining_route_data_when_there_is_no_route_data();
            await When_obtaining_route_data();
        }

        private static async Task When_obtaining_route_data_when_there_is_no_route_data()
        {
            var resp = await _client.GetAsync("route-data/noRouteData");
            resp.EnsureSuccessStatusCode();

            var headers = resp.Content.Headers.ToArray();

            headers.Length.ShouldBe(1);
            headers.ShouldContain(h => h.Key == "Content-Length");
            headers[0].Value.First().ShouldBe("0");

            var resStr = await resp.Content.ReadAsStringAsync();
            resStr.ShouldBeEmpty();
        }

        private static async Task When_obtaining_route_data()
        {
            var resp = await _client.GetAsync("route-data/myController/myAction/1");
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