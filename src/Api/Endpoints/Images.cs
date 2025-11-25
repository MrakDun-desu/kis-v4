using FileTypeChecker;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.Api.Endpoints;

public static class Images {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        routeBuilder.MapPost("images", Upload).DisableAntiforgery();
    }

    private static Results<Created, ValidationProblem> Upload(
        IFormFile image
    ) {
        // validating the filetype with magic bytes
        if (!FileTypeValidator.IsImage(image.OpenReadStream())) {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                { { nameof(image), ["File must be of type image"] } });
        }

        var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
        if (!Directory.Exists(imagesPath)) {
            Directory.CreateDirectory(imagesPath);
        }

        string filePath, fileName;
        do {
            // if file is actually an image, trust the extension to be correct,
            // so it's simpler to create a file with a correct extension
            fileName = Path.GetFileNameWithoutExtension(image.FileName) + "_" +
                       Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) +
                       Path.GetExtension(image.FileName);

            filePath = Path.Combine(imagesPath, fileName);
        } while (File.Exists(filePath));

        using var stream = File.Create(filePath);
        image.CopyTo(stream);

        return TypedResults.Created(Path.Combine("/images", fileName));
    }
}
