using Inventory.Domain.Entities;
using Inventory.Domain.Models.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inventory.Api.Attributes;

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
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous) return;

        var user = (Clients)context.HttpContext.Items["Client"];
        if (user == null || (_roles.Any() && !_roles.Contains(user.Role)))
        {
            context.Result = new JsonResult(new { message = "Not Allowed" })
                { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}