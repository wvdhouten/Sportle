using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Sportle.Web.Models.Users;

namespace Sportle.Web.Identity.Filters
{
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly UserRole[] _roles;

        public RoleAuthorizeAttribute(params UserRole[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var roleClaim = context.HttpContext.User.FindFirst("role");
            if (roleClaim is null)
            {
                context.Result = new RedirectToPageResult("/Account/Login");
                return;
            }

            var userRole = Enum.Parse<UserRole>(roleClaim.Value);
            foreach (var role in _roles)
            {
                if (role == userRole)
                {
                    return;
                }
            }

            context.Result = new ForbidResult();
        }
    }
}
