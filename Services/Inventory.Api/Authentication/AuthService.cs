using Authentication;
using Grpc.Core;
using User = Marketplace.Ordering.Ordering.API.User;

namespace Inventory.Api.Authentication;

/// <summary>
/// Service For validating token across Distributed services
/// </summary>
public class AuthService : Marketplace.Ordering.Ordering.API.AuthService.AuthServiceBase
{
    private readonly ITokenValidator _tokenValidator;

    public AuthService(ITokenValidator tokenValidator)
    {
        _tokenValidator = tokenValidator;
    }


    public override async Task<Marketplace.Ordering.Ordering.API.Result> ValidateToken(Marketplace.Ordering.Ordering.API.UserToken request,
        ServerCallContext context)
    {
        var result = await _tokenValidator.ValidateToken(new EventBus.Models.UserToken(request.Token));
        var user = new Marketplace.Ordering.Ordering.API.Result();
        if (result is null)
        {
            user.Code = 400;
            user.User = null;
        }
        else
        {
            user.Code = 200;
            user.User = new User
            {
                Id = result.userId.ToString(),
                FirstName = result.FirstName,
                LastName = result.LastName,
                Role = result.Role,
                Email = result.Email
            };
        }
        return user;
    }
}