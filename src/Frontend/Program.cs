using System.Net;
using Duende.AccessTokenManagement.OpenIdConnect;
using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();


builder.Services
    .AddBff()
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
    KnownProxies = { IPAddress.Parse("127.0.0.1") }
});
app.UseDefaultFiles();
app.MapStaticAssets();
app.UseRouting();
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapRemoteBffApiEndpoint("/api", new Uri("https://su-dev.fit.vutbr.cz/api"))
    .WithAccessToken();
app.MapRemoteBffApiEndpoint("/auth", new Uri("https://su-dev.fit.vutbr.cz"))
    .WithAccessToken();

app.Run();
