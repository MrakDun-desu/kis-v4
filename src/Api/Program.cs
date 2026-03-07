using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Audit.EntityFramework.Providers;
using FluentValidation;
using KisV4.Api.Endpoints;
using KisV4.Api.Middlewares;
using KisV4.BL.EF;
using KisV4.Common;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var applicationCulture = new CultureInfo("cs-CZ");
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

// Auth
const string oidcAuthority = "https://su-dev.fit.vutbr.cz/";
var allowTestingTokens = args.Contains("--testing-auth");
builder.Services.AddAuthentication(allowTestingTokens ? "Bearer" : "oidc")
    .AddJwtBearer("Bearer")
    .AddJwtBearer("oidc", opts => {
        opts.Authority = oidcAuthority;
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
            schema.Format = "int32";
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
            typeof(LayoutPipeModel)
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

// HTTP context accessor for getting the user ID during auditing
builder.Services.AddHttpContextAccessor();

// Time
builder.Services.AddSingleton(TimeProvider.System);

// Formatting
builder.Services.ConfigureHttpJsonOptions(opts => {
    opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// ValidatorOptions.Global.PropertyNameResolver = (type, memberInfo, expression) => {
//     if (expression is null) {
//         return memberInfo?.Name;
//     }
//     var chain = FluentValidation.Internal.PropertyChain.FromExpression(expression);
//     // For requests that nest models inside, remove the model so the errors are easier to read
//     if (chain.Count > 0) {
//         return chain.ToString().Replace("Model.", string.Empty);
//     }
//     return memberInfo?.Name;
// };

// Production exception handling
if (!builder.Environment.IsDevelopment()) {
    builder.Services.AddExceptionHandler<ExceptionHandlerMiddleware>();
}

var app = builder.Build();

// Auditing
var contextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
Audit.Core.Configuration.DataProvider = new EntityFrameworkDataProvider(opts => {
    opts
        .AuditTypeMapper(t => typeof(AuditLog))
        .AuditEntityAction<AuditLog>(async (auditEvent, entry, entity) => {
            var context = contextAccessor.HttpContext!;
            var claims = context.User;

            entity.UserId = claims.GetUserId();

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
app.UseHttpsRedirection();
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

app.Run();
