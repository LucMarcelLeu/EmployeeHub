using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace EmployeeHub.Api.Security;

public class KeycloakRoleClaimsTransformation
    : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identity as ClaimsIdentity;
        if (identity == null)
        {
            return Task.FromResult(principal);
        }

        var realmAccess = identity.FindFirst("realm_access");
        if (realmAccess != null)
        {
            // JSON aus realm_access lesen
            using var document = System.Text.Json.JsonDocument.Parse(realmAccess.Value);

            if (document.RootElement.TryGetProperty("roles", out var roles))
            {
                foreach (var role in roles.EnumerateArray())
                {
                    identity.AddClaim(
                        new Claim(
                            ClaimTypes.Role,
                            role.GetString()!));
                }
            }
        }

        return Task.FromResult(principal);
    }
}