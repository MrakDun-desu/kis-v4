using FluentValidation;

namespace KisV4.Api.RouteFilters;

public class ValidationFilter<T> : IEndpointFilter {
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    ) {
        var typeName = typeof(T).Name;
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>() ??
            throw new InvalidOperationException($"""
                Tried using a nonexistent validator for type {typeName}
                """);

        var req = context.Arguments.OfType<T>().FirstOrDefault() ??
            throw new InvalidOperationException($"""
                Tried using a validator for type {typeName} on an endpoint that doesn't accept
                any arguments of given type
                """);

        var validationResult = await validator.ValidateAsync(req);
        if (!validationResult.IsValid) {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        return await next(context);
    }
}
