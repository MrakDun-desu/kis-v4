using Duende.Bff;
using Duende.Bff.Yarp;

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
        options.ClientId = "kis-sales";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.Scope.Remove("profile"); // normal profile not supported by KIS Auth
        string[] requiredScopes = [
            "offline_access",
            "openid",
            "roles",
            "cpo",
            "fpo"
        ];
        foreach (var scope in requiredScopes) {
            options.Scope.Add(scope);
        }
        if (builder.Environment.IsDevelopment()) {
            options.BackchannelHttpHandler = developmentHandler;
        }
    });

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapRemoteBffApiEndpoint("/api", new Uri("https://localhost:7001"))
    .WithAccessToken();

app.Run();
