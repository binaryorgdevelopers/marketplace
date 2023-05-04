using Authentication.Enum;
using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Events;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Entities;
using Inventory.Domain.Extensions;
using Inventory.Domain.Models.Constants;
using Inventory.Domain.Shared;
using Microsoft.AspNetCore.Identity;

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

    public async Task<Result<AuthResult>> Handle(SellerSignInCommand request, CancellationToken cancellationToken)
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
                    Roles.Seller), _tokenGenerator.GenerateToken(seller.ToTokenRequest()))
        );
    }
}