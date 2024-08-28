using System.Data;
using KisV4.App;
using KisV4.App.Configuration;
using KisV4.App.Endpoints;
using KisV4.BL.EF;
using KisV4.DAL.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Image storage
var imageDirectory = builder.Configuration.GetValue<string>("ImageDirectory")
                     ?? Path.Combine(Environment.CurrentDirectory, "Images");
builder.Services.AddSingleton(new ImageStorageConfiguration(imageDirectory));

// Auth
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

// OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new NoNullAllowedException("Database connection string");
builder.Services.AddEntityFrameworkDAL(connectionString);
builder.Services.AddEntityFrameworkBL();

// Time
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

// Middlewares
app.UseCors();
app.UseHttpsRedirection();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imageDirectory),
    RequestPath = "/images"
});
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
Discounts.MapEndpoints(app);
DiscountUsages.MapEndpoints(app);
Images.MapEndpoints(app);
Modifiers.MapEndpoints(app);
Pipes.MapEndpoints(app);
SaleItems.MapEndpoints(app);
StoreItems.MapEndpoints(app);
Stores.MapEndpoints(app);

app.Run();