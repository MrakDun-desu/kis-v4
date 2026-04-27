using System.Net;
using Duende.AccessTokenManagement.OpenIdConnect;
using Duende.Bff;
using Duende.Bff.Yarp;
using KisV4.Frontend.Configuration;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services
    .AddBff()
    .AddRemoteApis();

var kisSettings = builder.Configuration.GetRequiredSection("Kis").Get<KisSettings>()!;

builder.Services
    .AddAuthentication(options => {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options => {
        options.Authority = kisSettings.AuthUrl;
        options.ClientId = kisSettings.ClientId;
        options.ClientSecret = kisSettings.ClientSecret;
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.MapInboundClaims = false;
        options.Scope.Remove("profile"); // normal profile not supported by KIS Auth
        string[] requiredScopes = [
            "openid",
            "roles",
            "fpo",
            "offline_access",
            "rfid:legacy:r",
            "rfid:pool:w",
            "kf:r",
            "kf:w"
        ];
        foreach (var scope in requiredScopes) {
            options.Scope.Add(scope);
        }
    });

builder.Services.AddOpenIdConnectAccessTokenManagement();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}

app.UseForwardedHeaders(new() {
    ForwardedHeaders = ForwardedHeaders.All,
    KnownIPNetworks = { new System.Net.IPNetwork(IPAddress.Parse("172.16.0.0"), 12) }
});

app.UsePathBase(new PathString(kisSettings.PathBase));
app.UseDefaultFiles();
app.MapStaticAssets();
app.UseRouting();
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapRemoteBffApiEndpoint("/api", new Uri(kisSettings.BackendUrl))
    .WithAccessToken();
app.MapRemoteBffApiEndpoint("/auth", new Uri(kisSettings.AuthUrl))
    .WithAccessToken();

app.Run();
