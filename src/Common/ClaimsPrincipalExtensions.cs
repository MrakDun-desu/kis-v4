using System.Data;
using System.Security.Claims;

namespace KisV4.Common;

public static class ClaimsPrincipalExtensions {
    public static int? TryGetUserId(this ClaimsPrincipal self) =>
        self.Identity?.Name switch {
            null => null,
            var val => int.Parse(val)
        };

    public static int GetUserId(this ClaimsPrincipal self) =>
        int.Parse(self.Identity!.Name!);
}
