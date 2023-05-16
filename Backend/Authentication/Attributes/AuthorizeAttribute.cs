using Authentication.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Authentication.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AddRolesAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<Roles> _roles;

        public AddRolesAttribute(params Roles[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context
                .ActionDescriptor
                .EndpointMetadata
                .OfType<AllowAnonymousAttribute>()
                .Any();

            if (allowAnonymous) return;

            var user = (dynamic)context.HttpContext.Items["User"]!;
            if (user is null)
            {
                context.Result = new JsonResult(new { message = "Not Authenticated" })
                    { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            _ = user.Role;
            // _ = System.Enum.TryParse(user.Role, out Roles role);
            // Roles role = (Roles)System.Enum.Parse(typeof(Roles), user.Role);
            var isExist = _roles.Contains(user.Role);
            if (_roles.Any() && !isExist)
            {
                context.Result = new JsonResult(new { message = "Not Allowed" })
                    { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}