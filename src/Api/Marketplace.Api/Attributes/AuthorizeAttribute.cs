using Marketplace.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Marketplace.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute
{
    private readonly IList<RoleEnum> _roles;

    public AuthorizeAttribute(params RoleEnum[] roles)
    {
        _roles = roles ?? Array.Empty<RoleEnum>();
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous) return;

        var user = (User)context.HttpContext.Items["users"];
        if (user == null || (_roles.Any() && _roles.Contains(user.Role)))
        {
            context.Result = new JsonResult(new { message = "Unauthorized" })
                { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}

