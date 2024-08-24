using FileTypeChecker;
using KisV4.App.Configuration;
using KisV4.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Images
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        // ignoring anti-forgery for now, maybe include it in the future?
        routeBuilder.MapPost("images", Upload).DisableAntiforgery();
    }

    private static Results<Created, ValidationProblem> Upload(
        IFormFile image,
        ImageStorageConfiguration conf,
        HttpRequest request)
    {
        // validating the filetype with magic bytes
        if (!FileTypeValidator.IsImage(image.OpenReadStream()))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                { { "fileType", ["File must be of type image"] } });
        }

        string creationPath;
        string fileName;
        do
        {
            // if file is actually an image, trust the extension to be correct,
            // so it's simpler to create a file with a correct extension
            fileName = Path.GetFileNameWithoutExtension(image.FileName) + "_" +
                       Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) +
                       Path.GetExtension(image.FileName);

            creationPath = Path.Combine(conf.Path, fileName);
        } while (File.Exists(creationPath));

        using var stream = File.Create(creationPath);
        image.CopyTo(stream);

        return TypedResults.Created(request.Host + "/images/" + fileName);
    }
}