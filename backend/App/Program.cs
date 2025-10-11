using System.Data;
using System.Reflection;
using KisV4.App.Configuration;
using KisV4.App.Endpoints;
using KisV4.BL.EF;
using KisV4.DAL.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(static opts => {
    opts.AddDefaultPolicy(static policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Custom configuration
builder.Services.Configure<ImageStorageSettings>(
    builder.Configuration.GetSection("ImageStorage")
);
builder.Services.Configure<ScriptStorageSettings>(
    builder.Configuration.GetSection("ScriptStorage")
);

// Auth
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

// OpenAPI
builder.Services.AddOpenApi(opts => {
    opts.AddDocumentTransformer((doc, _, _) => {
        doc.Info.Title = "KISv4 API";
        doc.Components ??= new OpenApiComponents();
        doc.Components.SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme> {
            ["Bearer"] = new() {
                Name = "Bearer",
                Description = "JWT Authorization header using the bearer scheme",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = "bearer",
                BearerFormat = "JWT"
            },
        };

        var requirement = new OpenApiSecurityRequirement {
            [new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme,
                }
            }] = Array.Empty<string>()
        };
        foreach (var op in doc.Paths.Values.SelectMany(path => path.Operations)) {
            op.Value.Security.Add(requirement);
        }

        return Task.CompletedTask;
    });
});

// Database
if (Assembly.GetEntryAssembly()?.GetName().Name == "GetDocument.Insider") {
    // if just running through the document generator, add empty dbContext
    builder.Services.AddDbContext<KisDbContext>();
} else {
    // if running properly, add the database from config
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new NoNullAllowedException("Database connection string");
    builder.Services.AddEntityFrameworkDAL(connectionString);
}
builder.Services.AddEntityFrameworkBL();

// Time
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

// Middlewares
app.UseCors();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
CashBoxes.MapEndpoints(app);
Categories.MapEndpoints(app);
Compositions.MapEndpoints(app);
Containers.MapEndpoints(app);
ContainerTemplates.MapEndpoints(app);
Costs.MapEndpoints(app);
Currencies.MapEndpoints(app);
CurrencyChanges.MapEndpoints(app);
Discounts.MapEndpoints(app);
DiscountUsages.MapEndpoints(app);
Images.MapEndpoints(app);
Modifiers.MapEndpoints(app);
Pipes.MapEndpoints(app);
SaleItemAmounts.MapEndpoints(app);
SaleItems.MapEndpoints(app);
SaleTransactions.MapEndpoints(app);
StoreItemAmounts.MapEndpoints(app);
StoreItems.MapEndpoints(app);
StoreTransactions.MapEndpoints(app);
Stores.MapEndpoints(app);
Users.MapEndpoints(app);

// OpenAPI
app.MapOpenApi().AllowAnonymous();
app.MapScalarApiReference().AllowAnonymous();

app.Run();
