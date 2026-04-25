using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Audit.EntityFramework.Providers;
using Duende.AccessTokenManagement;
using KisV4.Api.Endpoints;
using KisV4.Api.Middlewares;
using KisV4.BL.EF;
using KisV4.Common.Authorization;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using KisV4.DAL.EF.Seeding;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var applicationCulture = new CultureInfo("cs");
CultureInfo.CurrentCulture = applicationCulture;
CultureInfo.DefaultThreadCurrentCulture = applicationCulture;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(opts => {
    opts.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var kisSettings = builder.Configuration.GetRequiredSection("Kis").Get<KisSettings>()!;
builder.Services.Configure<KisSettings>(builder.Configuration.GetRequiredSection("Kis"));

// Auth
var oidcAuthority = kisSettings.AuthUrl;
var allowTestingTokens = args.Contains("--testing-auth");
builder.Services.AddAuthentication(allowTestingTokens ? "Bearer" : "oidc")
    .AddJwtBearer("Bearer")
    .AddJwtBearer("oidc", opts => {
        opts.Authority = oidcAuthority;
        opts.TokenValidationParameters.ValidateAudience = false;
        opts.TokenValidationParameters.NameClaimType = "sub";
        opts.TokenValidationParameters.RoleClaimType = "role";
        opts.MapInboundClaims = false;
        opts.SaveToken = true;
    });

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
if (!allowTestingTokens) {
    builder.Services.AddScoped<IClaimsTransformation, UserInfoClaimsTransformation>();
}

// OpenAPI
builder.Services.AddOpenApi(opts => {
    // document transformer to make authorization work correctly in scalar
    opts.AddDocumentTransformer((doc, _, _) => {
        doc.Info.Title = "KISv4 API";
        doc.Info.Version = "1.0.0";
        doc.Components ??= new OpenApiComponents();
        doc.Security ??= [];
        doc.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
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
            doc.Security.Add(new() {
                [new OpenApiSecuritySchemeReference("Bearer", doc)] = []
            });
        }

        doc.Components.SecuritySchemes["oidc"] = new OpenApiSecurityScheme {
            Name = "Authorization",
            Description = "OpenID Connect authentication via KIS.Auth",
            Type = SecuritySchemeType.OpenIdConnect,
            In = ParameterLocation.Header,
            OpenIdConnectUrl = new Uri($"{oidcAuthority}.well-known/openid-configuration/"),
            Scheme = "openIdConnect"
        };
        doc.Security.Add(new() {
            [new OpenApiSecuritySchemeReference("oidc", doc)] = []
        });
        return Task.CompletedTask;
    });

    // schema transformer to make numbers work correctly in JS
    opts.AddSchemaTransformer((schema, context, cancellationToken) => {
        if (context.JsonTypeInfo.Type == typeof(decimal) ||
            context.JsonTypeInfo.Type == typeof(decimal?)) {
            schema.Type = JsonSchemaType.String;
            schema.Format = "string";
            schema.AnyOf = null;
        }
        // remove options from integers and just pass them down as numbers so JS generator isn't confused
        if (context.JsonTypeInfo.Type == typeof(int) ||
            context.JsonTypeInfo.Type == typeof(int?)) {
            schema.Type = JsonSchemaType.Number;
            schema.Format = "number";
            schema.AnyOf = null;
        }

        return Task.CompletedTask;
    });

    // schema transformer to make the polymorphic types work in JS
    opts.AddSchemaTransformer((schema, context, cancellationToken) => {
        if (context.JsonTypeInfo.Type == typeof(LayoutItemModel)) {
            schema.Required ??= new HashSet<string>();
            schema.Required.Add("type");
        }

        Type[] derivedTypes = [
            typeof(LayoutSaleItemModel),
            typeof(LayoutLinkModel),
            typeof(LayoutTapModel)
        ];

        if (derivedTypes.Contains(context.JsonTypeInfo.Type)) {
            schema.Required ??= new HashSet<string>();
            schema.Required.Add("type");
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

// Business layer (services, validation, authorization handlers)
builder.Services.AddEntityFrameworkBL();

// HTTP context accessor - usable basically everywhere
// validation, authorization, automatic role creation, auditing... all need the current HTTP context
builder.Services.AddHttpContextAccessor();

// HTTP client and memory cache for requesting UserInfo from the authorization server
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

// HTTP client for accessing KIS Food with the client access token
builder.Services.AddClientCredentialsTokenManagement()
       .AddClient(AuthorizationConstants.KisFoodHttpClientName, client => {
           client.TokenEndpoint = new Uri(kisSettings.AuthUrl + "connect/token");
           client.ClientId = ClientId.Parse(kisSettings.ClientId);
           client.ClientSecret = ClientSecret.Parse(kisSettings.ClientSecret);
           client.Scope = Duende.AccessTokenManagement.Scope.Parse("kf:w");
       });
builder.Services.AddClientCredentialsHttpClient(
        AuthorizationConstants.KisFoodHttpClientName,
        ClientCredentialsClientName.Parse(AuthorizationConstants.KisFoodHttpClientName),
    client => {
        client.BaseAddress = new Uri(
            kisSettings.FoodUrl ?? $"{kisSettings.AuthUrl}food/"
        );
    });

// Time
builder.Services.AddSingleton(TimeProvider.System);

// Formatting
builder.Services.ConfigureHttpJsonOptions(opts => {
    opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Exception handling
builder.Services.AddExceptionHandler<ExceptionHandlerMiddleware>();

var app = builder.Build();

// Seeding
if (args.Contains("--migrate-db")) {
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<KisDbContext>();
    dbContext.Database.EnsureCreated();
}

if (args.Contains("--test-seed")) {
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<TestSeeder>();

    Audit.Core.Configuration.AuditDisabled = true;
    seeder.Seed();
    Audit.Core.Configuration.AuditDisabled = false;
}

// Auditing
var contextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
Audit.Core.Configuration.DataProvider = new EntityFrameworkDataProvider(opts => {
    opts
        .AuditTypeMapper(t => typeof(AuditLog))
        .AuditEntityAction<AuditLog>(async (auditEvent, entry, entity) => {
            var context = contextAccessor.HttpContext!;
            var claims = context.User;

            entity.UserId = claims.Identity?.Name;
            entity.EntityType = entry.EntityType.Name;
            entity.Action = entry.Action;
            entity.Changes = JsonSerializer.SerializeToDocument(entry.Changes);
            entity.EntityKeys = JsonSerializer.SerializeToDocument(entry.PrimaryKey);
            entity.StartDate = auditEvent.StartDate;
            entity.EndDate = auditEvent.EndDate;
        })
        .IgnoreMatchedProperties(true);
});

// Middlewares
app.UseCors();
app.UseRouting();
if (!app.Environment.IsDevelopment()) {
    app.UseHsts();
}
app.UseAuthentication();
app.UseAuthorization();
// users are not managed by this API, so just create a new one every time a new ID arrives
app.UseMiddleware<UserCreationMiddleware>();
app.UseStaticFiles(); // static files only for serving images

// Endpoints
AccountTransactions.MapEndpoints(app);
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
Taps.MapEndpoints(app);
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

app.Run();
