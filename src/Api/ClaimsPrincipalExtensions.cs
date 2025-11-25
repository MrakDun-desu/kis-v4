using System.Data;
using System.Security.Claims;

namespace KisV4.Api;

public static class ClaimsPrincipalExtensions {
    public static int GetUserId(this ClaimsPrincipal self) =>
        self.FindFirstValue(ClaimTypes.NameIdentifier) switch {
            null => throw new NoNullAllowedException("User ID not present in token"),
            var val => int.Parse(val)
        };
}
