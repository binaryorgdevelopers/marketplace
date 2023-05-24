using Authentication.Enum;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Inventory.Domain.Extensions;
using Marketplace.Application.Common.Messages.Commands;
using Microsoft.AspNetCore.Identity;
using Shared.Abstraction.Messaging;
using Shared.Messages;
using Shared.Models;
using Shared.Models.Constants;

namespace Marketplace.Application.Queries.Query.Auth;

public class SellerSignInQueryHandler : ICommandHandler<SellerSignInCommand,AuthResult>
{
    private readonly IGenericRepository<Seller> _repository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHasher<Seller> _passwordHasher;

    public SellerSignInQueryHandler(IGenericRepository<Seller> repository, IJwtTokenGenerator tokenGenerator,
        IPasswordHasher<Seller> passwordHasher)
    {
        _repository = repository; 
        _tokenGenerator = tokenGenerator;
        _passwordHasher =  passwordHasher;
    }

    public async ValueTask<Result<AuthResult>> HandleAsync(SellerSignInCommand request, CancellationToken cancellationToken)
    {
        var seller = await _repository.GetAsync(c => c.Email == request.Email);
        if (seller is null)
            return Result.Failure<AuthResult>(new Error(Codes.UserNotFound,
                $"User with Email:'{request.Email}' not found"));

        var verificationResult = seller.ValidatePassword(request.Password, _passwordHasher);
        if (!verificationResult)
            return Result.Failure<AuthResult>(new Error(Codes.InvalidPassword, "Invalid password"));
        seller.LastSession = DateTime.Now.SetKindUtc();
        _repository.Update(seller);

        return Result.Success(
            new AuthResult(
                new Authorized(seller.Id, seller.FirstName, seller.LastName, seller.PhoneNumber, seller.Email,
                    Roles.Seller.ToString()), _tokenGenerator.GenerateToken(seller.ToTokenRequest()))
        );
    }
}