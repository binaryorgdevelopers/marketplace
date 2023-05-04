using Authentication.Enum;
using EventBus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

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

            if (_roles.Any() && !_roles.Contains((Roles)user.Role))
            {
                context.Result = new JsonResult(new { message = "Not Allowed" })
                    { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}