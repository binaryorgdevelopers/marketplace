using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Marketplace.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AddRolesAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<Roles> _roles;

    public AddRolesAttribute(params Roles[] roles)
    {
        _roles = roles ?? Array.Empty<Roles>();
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous) return;

        var user = (User)context.HttpContext.Items["User"];
        if (user == null || (_roles.Any() && !_roles.Contains(user.Role)))
        {
            context.Result = new JsonResult(new { message = "Not Allowed" })
                { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}