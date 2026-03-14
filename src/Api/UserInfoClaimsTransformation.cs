using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Net.Http.Headers;

public class UserInfoClaimsTransformation(
    IHttpClientFactory httpClientFactory,
    IMemoryCache cache,
    IHttpContextAccessor httpContextAccessor,
    IOptionsSnapshot<JwtBearerOptions> jwtOptions) : IClaimsTransformation {

    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IMemoryCache _cache = cache;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly JwtBearerOptions _jwtOptions = jwtOptions.Get("oidc");

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal) {
        // Check if user is authenticated or if we've already transformed this principal
        if (principal.Identity is not { IsAuthenticated: true } || principal.HasClaim(c => c.Type == "urn:transformed")) {
            return principal;
        }

        var sub = principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? principal.FindFirstValue("sub");
        if (string.IsNullOrEmpty(sub)) return principal;

        // Try to get user info from cache
        var cacheKey = $"UserInfo_{sub}";
        if (!_cache.TryGetValue(cacheKey, out List<Claim>? extraClaims)) {
            extraClaims = await FetchUserInfoFromProvider();

            if (extraClaims is not null && extraClaims.Any()) {
                _cache.Set(cacheKey, extraClaims, TimeSpan.FromHours(12));
            }
        }

        var clone = principal.Clone();
        var newIdentity = (ClaimsIdentity)clone.Identity!;

        if (extraClaims is not null) {
            newIdentity.AddClaims(extraClaims);
        }

        // Mark as transformed to prevent re-processing in the same request
        newIdentity.AddClaim(new Claim("urn:transformed", "true"));

        return clone;
    }

    private async Task<List<Claim>?> FetchUserInfoFromProvider() {
        // Get the Access Token from the current request
        const string bearerHeader = "Bearer ";
        var authHeader = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith(bearerHeader)) {
            return null;
        }

        var accessToken = authHeader[bearerHeader.Length..].Trim();

        var client = _httpClientFactory.CreateClient();

        try {
            // Find the UserInfo endpoint from the Authority
            var discoveryUrl = $"{_jwtOptions.Authority}.well-known/openid-configuration";
            var discoveryResponse = await client.GetFromJsonAsync<OidcDiscoveryDocument>(discoveryUrl);

            if (string.IsNullOrEmpty(discoveryResponse?.UserInfoEndpoint)) return null;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var userInfo = await client.GetFromJsonAsync<Dictionary<string, object>>(discoveryResponse.UserInfoEndpoint);

            if (userInfo == null) {
                return null;
            }

            return userInfo.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? "")).ToList();
        } catch {
            return null;
        }
    }

    private record OidcDiscoveryDocument([property: System.Text.Json.Serialization.JsonPropertyName("userinfo_endpoint")] string UserInfoEndpoint);
}
