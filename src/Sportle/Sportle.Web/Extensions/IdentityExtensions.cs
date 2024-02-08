using System.Security.Claims;

namespace Sportle.Web.Extensions
{
    public static class IdentityExtensions
    {
        public static bool HasId(this ClaimsPrincipal principal, out Guid userId)
        {
            return Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
        }
    }
}
