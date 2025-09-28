using KisV4.App.Configuration;
using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KisV4.App.Endpoints;

public static class Discounts {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("discounts");
        group.MapGet(string.Empty, ReadAll);
        group.MapGet("{id:int}", Read);
        group.MapPost(string.Empty, Create);
        group.MapPut("{id:int}", Put);
        group.MapDelete("{id:int}", Delete);
    }

    private static IEnumerable<DiscountListModel> ReadAll(
        IDiscountService discountService,
        [FromQuery] bool? deleted
    ) {
        return discountService.ReadAll(deleted);
    }

    private static Results<Ok<DiscountDetailModel>, NotFound> Read(
        IDiscountService discountService,
        int id
    ) {
        return discountService.Read(id).Match<Results<Ok<DiscountDetailModel>, NotFound>>(
            static result => TypedResults.Ok(result),
            static _ => TypedResults.NotFound()
        );
    }

    private static Results<Ok<DiscountDetailModel>, ValidationProblem> Create(
        IDiscountService discountService,
        IOptions<ScriptStorageSettings> conf,
        DiscountCreateModel createModel
    ) {
        return discountService.Create(createModel).Match<Results<Ok<DiscountDetailModel>, ValidationProblem>>(
            output => {
                var fileName = $"Discount{output.Id}-{output.Name}.cs";
                File.WriteAllText(
                    Path.Combine(conf.Value.Path, fileName),
                    createModel.Script
                );
                return TypedResults.Ok(output);
            },
            static errors => TypedResults.ValidationProblem(errors)
        );
    }

    private static Results<Ok<DiscountDetailModel>, NotFound> Put(
        IDiscountService discountService,
        int id
    ) {
        return discountService.Put(id).Match<Results<Ok<DiscountDetailModel>, NotFound>>(
            static result => TypedResults.Ok(result),
            static _ => TypedResults.NotFound()
        );
    }

    private static Results<Ok<DiscountDetailModel>, NotFound> Delete(
        IDiscountService discountService,
        int id
    ) {
        return discountService.Delete(id).Match<Results<Ok<DiscountDetailModel>, NotFound>>(
            static result => TypedResults.Ok(result),
            static _ => TypedResults.NotFound()
        );
    }
}
