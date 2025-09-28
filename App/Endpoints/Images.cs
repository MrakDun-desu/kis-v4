using FileTypeChecker;
using KisV4.App.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KisV4.App.Endpoints;

public static class Images {
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        // ignoring anti-forgery for now, maybe include it in the future?
        routeBuilder.MapPost("images", Upload).DisableAntiforgery();
        routeBuilder.MapGet("images/{filename}", Download).DisableAntiforgery();
    }

    private static Results<Created, ValidationProblem> Upload(
        IFormFile image,
        ImageStorageConfiguration conf,
        HttpRequest request) {
        // validating the filetype with magic bytes
        if (!FileTypeValidator.IsImage(image.OpenReadStream())) {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
                { { nameof(image), ["File must be of type image"] } });
        }

        string creationPath, fileName;
        do {
            // if file is actually an image, trust the extension to be correct,
            // so it's simpler to create a file with a correct extension
            fileName = Path.GetFileNameWithoutExtension(image.FileName) + "_" +
                       Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) +
                       Path.GetExtension(image.FileName);

            creationPath = Path.Combine(conf.Path, fileName);
        } while (File.Exists(creationPath));

        using var stream = File.Create(creationPath);
        image.CopyTo(stream);

        return TypedResults.Created(request.Host + request.Path + "/" + fileName);
    }

    private static Results<NotFound, PhysicalFileHttpResult> Download(
        string filename,
        ImageStorageConfiguration conf) {
        var filePath = Path.Combine(conf.Path, filename);
        if (!File.Exists(filePath)) {
            return TypedResults.NotFound();
        }

        var extension = Path.GetExtension(filePath);
        var mimetype = GetImageMimeType(extension);
        var lastModified = File.GetLastWriteTimeUtc(filePath);

        return TypedResults.PhysicalFile(filePath, mimetype, lastModified: lastModified);
    }

    private static string GetImageMimeType(string extension) => extension switch {
        ".png" => "image/png",
        ".gif" => "image/gif",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".bmp" => "image/bmp",
        ".tiff" => "image/tiff",
        ".wmf" => "image/wmf",
        ".jp2" => "image/jp2",
        ".svg" => "image/svg+xml",
        ".webp" => "image/webp",
        _ => "application/octet-stream",
    };
}
