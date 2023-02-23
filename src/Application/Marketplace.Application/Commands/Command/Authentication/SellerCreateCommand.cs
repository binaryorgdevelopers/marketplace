using Marketplace.Application.Commands.ICommand.Authentication;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Constants;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Commands.Command.Authentication;

public class SellerCreateCommand : ISellerCreateCommand
{
    private readonly IGenericRepository<Seller> _genericRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHasher<Seller> _passwordHasher;

    public SellerCreateCommand(IGenericRepository<Seller> genericRepository, IJwtTokenGenerator tokenGenerator,
        IPasswordHasher<Seller> passwordHasher)
    {
        _genericRepository = genericRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Either<AuthResult, Exception>> CreateSeller(SellerCreate sellerCreate)
    {
        var isExist = await _genericRepository.ExistsAsync(c => c.Username == sellerCreate.Username);
        if (isExist)
            return new Either<AuthResult, Exception>(new AuthException(Codes.InvalidCredential,
                $"User with Email:'{sellerCreate.Email}' is already exist"));

        Seller seller = new Seller(Guid.NewGuid(), sellerCreate.PhoneNumber, sellerCreate.Email, sellerCreate.Title,
            sellerCreate.Description, sellerCreate.Info, sellerCreate.Username, sellerCreate.FirstName,
            sellerCreate.LastName, null, null, null);
        seller.SetPassword(sellerCreate.Password,_passwordHasher);
        await _genericRepository.AddAsync(seller);

        return new Either<AuthResult, Exception>(new AuthResult(new Authorized(seller.Id, seller.FirstName,
                seller.LastName, seller.PhoneNumber, seller.Email, Roles.Seller),
            _tokenGenerator.GenerateToken(seller.ToTokenRequest())));
    }
}