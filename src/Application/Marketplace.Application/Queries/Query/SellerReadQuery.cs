using Marketplace.Application.Common;
using Marketplace.Application.Common.Extensions;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Application.Queries.IQuery;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Queries.Query;

public class SellerReadQuery : ISellerReadQuery
{
    private readonly IGenericRepository<Seller> _repository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHasher<Seller> _passwordHasher;

    public SellerReadQuery(IGenericRepository<Seller> repository, IJwtTokenGenerator tokenGenerator,
        IPasswordHasher<Seller> passwordHasher)
    {
        _repository = repository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public Either<AuthResult, Exception> SellerSignIn(SignInCommand signInCommand)
    {
        var seller = _repository.Get(c => c.Email == signInCommand.Email);
        if (seller is null)
            return new Either<AuthResult, Exception>(new AuthException(Codes.UserNotFound,
                $"User with Email:'{signInCommand.Email}' not found"));

        var verificationResult = seller.ValidatePassword(signInCommand.Password, _passwordHasher);
        if (!verificationResult)
            return new Either<AuthResult, Exception>(new AuthException(Codes.InvalidPassword, "Invalid password"));
        seller.LastSession = DateTime.Now.SetKindUtc();
        _repository.Update(seller);

        return new Either<AuthResult, Exception>(
            new AuthResult(
                new Authorized(seller.Id, seller.FirstName, seller.LastName, seller.PhoneNumber, seller.Email,
                    Roles.Seller), _tokenGenerator.GenerateToken(seller.ToTokenRequest()))
        );
    }
}