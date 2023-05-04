using Authentication;
using EventBus.Models;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;

namespace Marketplace.Infrastructure.Services;

public class TokenValidatorService : ITokenValidator
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IGenericRepository<Clients> _userRepository;

    public TokenValidatorService(IJwtTokenGenerator jwtTokenGenerator, IGenericRepository<Clients> userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public Task<UserDto?> ValidateToken(UserToken userToken)
        => Task.Run(() =>
        {
            Guid? guid = _jwtTokenGenerator.ValidateJwtToken(userToken.Token.Substring(7));
            if (guid is null) return null;
            var user = _userRepository.Get(u => u.Id == guid.Value).FirstOrDefault();
            return new UserDto(user.Id, user.FirstName, user.LastName, user.Email, user.Role.ToString());
        });
}