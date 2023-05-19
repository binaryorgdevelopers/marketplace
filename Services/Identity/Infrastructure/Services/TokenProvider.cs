using System.Collections.Specialized;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Shared.Models;

namespace Identity.Infrastructure.Services;

internal class TokenProvider : ITokenProvider
{
    private readonly ITokenService _tokenService;
    private readonly IClientStore _clientStore;


    public TokenProvider(ITokenService tokenService, IClientStore clientStore)
    {
        _tokenService = tokenService;
        _clientStore = clientStore;
    }

    public async ValueTask<(string Token, int ExpiresAt)> GenerateToken(TokenRequest tokenRequest)
    {
        var client = await _clientStore.FindClientByIdAsync("your_client_id");
        var request = new TokenCreationRequest
        {
            Subject = new ClaimsPrincipal(),
            ValidatedRequest = new ValidatedRequest
            {
                Client = client,
                Raw = new NameValueCollection
                {
                    { nameof(tokenRequest.FirstName), tokenRequest.FirstName },
                    { nameof(tokenRequest.PhoneNumber), tokenRequest.PhoneNumber },
                    { nameof(tokenRequest.LastName), tokenRequest.LastName },
                    { nameof(tokenRequest.Role), tokenRequest.Role },
                    { nameof(tokenRequest.Email), tokenRequest.Email }
                }
            }
        };
        var tokenResult = await _tokenService.CreateAccessTokenAsync(request);


        return new(tokenResult.ToString(), 123);
    }
}

public interface ITokenProvider
{
    ValueTask<(string Token, int ExpiresAt)> GenerateToken(TokenRequest tokenRequest);
}