using System.Data;
using System.Reflection;
using System.Security.Claims;
using KisV4.Api.Endpoints;
using KisV4.BL.EF;
using KisV4.DAL.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(opts => {
    opts.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Auth
var allowTestingTokens = args.Contains("--testing-auth");
builder.Services.AddAuthentication(allowTestingTokens ? "Bearer" : "oidc")
    .AddJwtBearer("Bearer")
    .AddJwtBearer("oidc", opts => {
        opts.Authority = "https://su-dev.fit.vutbr.cz";
        opts.TokenValidationParameters.ValidateAudience = false;
        if (builder.Environment.IsDevelopment()) {
            opts.BackchannelHttpHandler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
        }
    });

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

// OpenAPI
builder.Services.AddOpenApi(opts => {
    opts.AddDocumentTransformer((doc, _, _) => {
        doc.Info.Title = "KISv4 API";
        doc.Components ??= new OpenApiComponents();
        doc.Components.SecuritySchemes.Clear();
        if (allowTestingTokens) {
            doc.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme {
                Name = "Bearer",
                Description = "Testing bearer token",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Scheme = "bearer"
            };
            doc.SecurityRequirements.Add(new() {
                [new OpenApiSecurityScheme {
                    Reference = new() {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                }] = Array.Empty<string>()
            });
        }
        doc.Components.SecuritySchemes["oidc"] = new() {
            Type = SecuritySchemeType.OpenIdConnect,
            OpenIdConnectUrl = new Uri("https://su-dev.fit.vutbr.cz/.well-known/openid-configuration/"),
            Description = "OpenID Connect authentication via KIS.Auth",
            Name = "Authorization",
            In = ParameterLocation.Header
        };
        doc.SecurityRequirements.Add(new() {
            [new OpenApiSecurityScheme {
                Reference = new() {
                    Id = "oidc",
                    Type = ReferenceType.SecurityScheme
                }
            }] = Array.Empty<string>()
        });
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
app.UseStaticFiles(); // static files only for serving images

// Endpoints
CashBoxes.MapEndpoints(app);
Categories.MapEndpoints(app);
CompositeAmounts.MapEndpoints(app);
Compositions.MapEndpoints(app);
ContainerChanges.MapEndpoints(app);
Containers.MapEndpoints(app);
ContainerTemplates.MapEndpoints(app);
Costs.MapEndpoints(app);
Images.MapEndpoints(app);
Layouts.MapEndpoints(app);
Modifiers.MapEndpoints(app);
Pipes.MapEndpoints(app);
SaleItems.MapEndpoints(app);
SaleTransactions.MapEndpoints(app);
StoreItemAmounts.MapEndpoints(app);
StoreItems.MapEndpoints(app);
Stores.MapEndpoints(app);
StoreTransactions.MapEndpoints(app);
Users.MapEndpoints(app);

// OpenAPI
app.MapOpenApi().AllowAnonymous();
app.MapScalarApiReference().AllowAnonymous();

app.MapGet("identity", (ClaimsPrincipal user) => {
    return user.Claims.Select(u => new { u.Type, u.Value });
});

app.Run();
