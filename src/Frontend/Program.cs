using Duende.Bff.Yarp;

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
        options.ClientId = "kis-sales";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
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
        options.SaveTokens = true;
        if (builder.Environment.IsDevelopment()) {
            options.BackchannelHttpHandler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
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

app.MapBffManagementEndpoints();
app.MapRemoteBffApiEndpoint("/sales-api", "https://localhost:7001")
    .RequireAccessToken();

app.Run();
