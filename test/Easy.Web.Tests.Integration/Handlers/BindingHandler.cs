namespace Easy.Web.Tests.Integration.Handlers
{
    using System.Threading.Tasks;
    using Easy.Web.Core.Extensions;
    using Easy.Web.Core.Helpers;
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Routing;
    using Easy.Web.Tests.Integration.Models;
    using Microsoft.AspNetCore.Http;
    using Shouldly;

    internal sealed class BindingHandler : Handler
    {
        [Route(HttpMethod.GET, "bind/noQueryString")]
        public Task BindToNoQueryString(HttpContext context)
        {
            var dynDic = context.ReadFromQueryString();
            dynDic.Count.ShouldBe(0);
            return context.ReplyAsStatus(HttpStatusCode.Okay);
        }

        [Route(HttpMethod.GET, "bind/queryString")]
        public Task BindToQueryString(HttpContext context)
        {
            var dynDic = context.ReadFromQueryString();

            dynDic.Count.ShouldBe(2);
            
            var id = dynDic["ID"];
            var category = dynDic["category"];

            id.ShouldBe("10");
            category.ShouldBe("some Category");

            return context.ReplyAsText($"Id: {id} - Category: {category}");
        }

        [Route(HttpMethod.GET, "bind/queryStringDynamic")]
        public Task BindToQueryStringDynamic(HttpContext context)
        {
            dynamic dynDic = context.ReadFromQueryString();

            ((int)dynDic.Count).ShouldBe(2);

            var id = dynDic["ID"];
            var category = dynDic.category;

            id.ShouldBe("10");
            category.ShouldBe("some Category");

            return context.ReplyAsText($"Id: {id} - Category: {category}");
        }

        [Route(HttpMethod.GET, "bind/queryStringModel")]
        public Task BindToQueryStringModel(HttpContext context)
        {
            var model = context.ReadFromQueryString<SampleModel>();

            model.ShouldNotBeNull();
            model.Id.ShouldBe(10);
            model.Category.ShouldBe("some Category");

            return context.ReplyAsText($"Id: {model.Id} - Category: {model.Category}");
        }

        [Route(HttpMethod.GET, "bind/queryStringModelSelective")]
        public Task BindToQueryStringModelSelective(HttpContext context)
        {
            var model = context.ReadFromQueryString<SampleModel>("Category");

            model.ShouldNotBeNull();
            model.Id.ShouldBeNull();
            model.Category.ShouldBe("some Category");

            return context.ReplyAsText($"Category: {model.Category}");
        }

        [Route(HttpMethod.GET, "bind/bodyNoContentType")]
        public async Task BindToBodyNoContentType(HttpContext context)
        {
            var model = await context.ReadFromBody<SampleModel>().ConfigureAwait(false);
            model.ShouldBeNull();
            await context.ReplyAsStatus(HttpStatusCode.BadRequest).ConfigureAwait(false);
        }

        [Route(HttpMethod.POST, "bind/bodyHasContent")]
        public async Task BindToBodyWithContent(HttpContext context)
        {
            var model = await context.ReadFromBody<SampleModel>().ConfigureAwait(false);
            if (model == null)
            {
                await context.ReplyAsStatus(HttpStatusCode.BadRequest).ConfigureAwait(false);
            }

            await context.ReplyAsStatus(HttpStatusCode.Okay).ConfigureAwait(false);
        }

        [Route(HttpMethod.POST, "bind/bodyHasContentSelective")]
        public async Task BindToBodyWithContentSelective(HttpContext context)
        {
            var model = await context.ReadFromBody<SampleModel>("Category").ConfigureAwait(false);
            model.ShouldNotBeNull();
            model.Id.ShouldBeNull();
            model.Category.ShouldBe("some Category");

            await context.ReplyAsStatus(HttpStatusCode.Okay).ConfigureAwait(false);
        }

        [Route(HttpMethod.PUT, "bind/putting")]
        public async Task BindToBodyWithContentAsPut(HttpContext context)
        {
            var model = await context.ReadFromBody<SampleModel>().ConfigureAwait(false);
            model.ShouldNotBeNull();
            model.Id.ShouldBe(123);
            model.Category.ShouldBe("some Category");

            await context.ReplyAsStatus(HttpStatusCode.Okay).ConfigureAwait(false);
        }

        [Route(HttpMethod.POST, "bind/bodyHasInvalidContent")]
        public async Task BindToBodyWithInvalidContent(HttpContext context)
        {
            var model = await context.ReadFromBody<SampleModel>().ConfigureAwait(false);
            model.ShouldNotBeNull();
            model.Id.ShouldBeNull();
            model.Category.ShouldBeNull();
            
            await context.ReplyAsStatus(HttpStatusCode.Okay).ConfigureAwait(false);
        }
    }
}