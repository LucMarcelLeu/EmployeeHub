using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace EmployeeHub.Infrastructure.Security;

public class KeycloakClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identity as ClaimsIdentity;

        if (identity == null)
            return Task.FromResult(principal);

        var realmAccess = principal.FindFirst("realm_access")?.Value;

        if (realmAccess == null)
            return Task.FromResult(principal);

        using var doc = JsonDocument.Parse(realmAccess);

        if (!doc.RootElement.TryGetProperty("roles", out var roles))
            return Task.FromResult(principal);

        foreach (var role in roles.EnumerateArray())
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role.GetString()!));
        }

        return Task.FromResult(principal);
    }
}