using Marketplace.Application.Commands.ICommand.Authentication;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Abstractions.Services;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Marketplace.Domain.Models.Constants;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Commands.Command.Authentication;

public class SellerCreateCommand : ISellerCreateCommand
{
    private readonly IGenericRepository<Seller> _genericRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHasher<Seller> _passwordHasher;
    private readonly ICloudUploaderService _cloudUploaderService;

    public SellerCreateCommand(IGenericRepository<Seller> genericRepository, IJwtTokenGenerator tokenGenerator,
        IPasswordHasher<Seller> passwordHasher, ICloudUploaderService cloudUploaderService)
    {
        _genericRepository = genericRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
        _cloudUploaderService = cloudUploaderService;
    }

    public async Task<Either<AuthResult, Exception>> CreateSeller(SellerCreate sellerCreate)
    {
        var isExist = await _genericRepository.ExistsAsync(c => c.Username == sellerCreate.Username);
        if (isExist)
            return new Either<AuthResult, Exception>(new AuthException(Codes.InvalidCredential,
                $"User with Username:'{sellerCreate.Username}' is already exist"));

        Seller seller = new Seller(Guid.NewGuid(), sellerCreate.PhoneNumber, sellerCreate.Email, sellerCreate.Title,
            sellerCreate.Description, sellerCreate.Info, sellerCreate.Username, sellerCreate.FirstName,
            sellerCreate.LastName, null,
            await _cloudUploaderService.Upload(sellerCreate.Banner, sellerCreate.Banner.FileName),
            await _cloudUploaderService.Upload(sellerCreate.Avatar, sellerCreate.Avatar.FileName));
        seller.SetPassword(sellerCreate.Password, _passwordHasher);
        await _genericRepository.AddAsync(seller);

        return new Either<AuthResult, Exception>(new AuthResult(new Authorized(seller.Id, seller.FirstName,
                seller.LastName, seller.PhoneNumber, seller.Email, Roles.Seller),
            _tokenGenerator.GenerateToken(seller.ToTokenRequest())));
    }
}