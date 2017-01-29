namespace Easy.Web.Tests.Integration.Handlers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Easy.Web.Core.Extensions;
    using Easy.Web.Core.Helpers;
    using Easy.Web.Core.Models;
    using Easy.Web.Core.Routing;
    using Easy.Web.Tests.Integration.Models;
    using Microsoft.AspNetCore.Http;
    using Shouldly;

    internal sealed class ValidatingHandler : Handler
    {
        [Route(HttpMethod.POST, "validate/body/valid")]
        public async Task ValidModelPassed(HttpContext context)
        {
            var model = await context.ReadFromBody<SampleModel>();

            List<ValidationResult> validationResult;
            TryValidateModel(model, out validationResult).ShouldBeTrue();

            validationResult.ShouldBeEmpty();

            model.ShouldNotBeNull();
            model.Id.ShouldBe(1);
            model.Category.ShouldBe("Valid");
            
            await context.ReplyAsStatus(HttpStatusCode.Okay);
        }

        [Route(HttpMethod.POST, "validate/body/validPartial")]
        public async Task ValidPartialModelPassed(HttpContext context)
        {
            var model = await context.ReadFromBody<SampleModel>("Id", "Category");

            List<ValidationResult> validationResult;
            TryValidateModel(model, out validationResult).ShouldBeTrue();

            validationResult.ShouldBeEmpty();

            model.ShouldNotBeNull();
            model.Id.ShouldBe(1);
            model.Category.ShouldBe("Valid");

            await context.ReplyAsStatus(HttpStatusCode.Okay);
        }

        [Route(HttpMethod.POST, "validate/body/invalid")]
        public async Task InValidModelPassed(HttpContext context)
        {
            var model = await context.ReadFromBody<SampleModel>();

            List<ValidationResult> validationResult;
            TryValidateModel(model, out validationResult).ShouldBeFalse();

            validationResult.ShouldNotBeEmpty();
            validationResult.Count.ShouldBe(2);
            validationResult[0].ErrorMessage.ShouldBe("The Id field is required.");
            validationResult[0].MemberNames.ShouldContain("Id");

            validationResult[1].ErrorMessage.ShouldBe("Maximum length failed.");
            validationResult[1].MemberNames.ShouldContain("Category");

            model.ShouldNotBeNull();
            model.Id.ShouldBeNull();
            model.Category.ShouldBe("Invalid Category");

            await context.SerializeAsJSON(validationResult, HttpStatusCode.BadRequest);
        }

        [Route(HttpMethod.POST, "validate/body/invalidPartial")]
        public async Task InValidPartialModelPassed(HttpContext context)
        {
            var model = await context.ReadFromBody<SampleModel>("Category");

            List<ValidationResult> validationResult;
            TryValidateModel(model, out validationResult).ShouldBeFalse();

            validationResult.ShouldNotBeEmpty();
            validationResult.Count.ShouldBe(1);
            validationResult[0].ErrorMessage.ShouldBe("The Id field is required.");
            validationResult[0].MemberNames.ShouldContain("Id");

            model.ShouldNotBeNull();
            model.Id.ShouldBeNull();
            model.Category.ShouldBe("Valid");

            await context.SerializeAsJSON(validationResult, HttpStatusCode.BadRequest);
        }
    }
}