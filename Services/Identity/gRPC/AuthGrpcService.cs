using Grpc.Core;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Services;
using Identity.Services;

namespace Identity.gRPC;

public class AuthGrpcService : AuthService.AuthServiceBase
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserRepository _userRepository;

    public AuthGrpcService(ITokenProvider tokenProvider, IUserRepository userRepository)
    {
        _tokenProvider = tokenProvider;
        _userRepository = userRepository;
    }

    public override async Task<GrpcResult> ValidateToken(GrpcToken request, ServerCallContext context)
    {
        var userId = _tokenProvider.ValidateJwtToken(request.Token[7..]);
        var grpcResult = new GrpcResult();
        if (userId is null)
        {
            grpcResult.Code = 404;
            grpcResult.User = null;
            return grpcResult;
        }

        var user = await _userRepository.GetUserById(userId.Value);

        grpcResult.Code = 200;
        grpcResult.User = new User
        {
            FirstName = user?.FirstName,
            LastName = user?.LastName,
            Email = user?.Email,
            Id = user?.Id.ToString(),
            Role = user?.Role.Name
        };
        return grpcResult;
    }
}