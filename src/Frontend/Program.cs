using Duende.AccessTokenManagement.OpenIdConnect;
using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

var developmentHandler = new HttpClientHandler {
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

builder.Services
    .AddBff(opts => {
        if (builder.Environment.IsDevelopment()) {
            opts.BackchannelHttpHandler = developmentHandler;
        }
    })
    .AddRemoteApis();

builder.Services
    .AddAuthentication(options => {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options => {
        options.Authority = "https://su-dev.fit.vutbr.cz";
        options.ClientId = "kis_frontend";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.MapInboundClaims = false;
        options.Scope.Remove("profile"); // normal profile not supported by KIS Auth
        string[] requiredScopes = [
            "openid",
            "roles",
            "fpo",
            "offline_access"
            // TODO add scopes for sales API
        ];
        foreach (var scope in requiredScopes) {
            options.Scope.Add(scope);
        }
        if (builder.Environment.IsDevelopment()) {
            options.BackchannelHttpHandler = developmentHandler;
        }
    });

builder.Services.AddOpenIdConnectAccessTokenManagement();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}

app.UseDefaultFiles();
app.MapStaticAssets();
app.UseRouting();
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapRemoteBffApiEndpoint("/api", new Uri("https://localhost:7001"))
    .WithAccessToken();

app.MapGet("/bff/logout", async (HttpContext ctx) => {
    await ctx.SignOutAsync("Cookies");
    await ctx.SignOutAsync("oidc");
}).RequireAuthorization();

app.Run();
