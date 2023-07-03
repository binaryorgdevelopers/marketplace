using Authentication.Enum;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Abstractions.Services;
using Inventory.Domain.Entities;
using Marketplace.Application.Common.Messages.Commands;
using Microsoft.AspNetCore.Identity;
using Shared.Abstraction.Messaging;
using Shared.Messages;
using Shared.Models;

namespace Marketplace.Application.Commands.Command.Authentication;

public class SellerCreateCommandHandler : ICommandHandler<SellerCreate, AuthResult>
{
    private readonly ICloudUploaderService _cloudUploaderService;
    private readonly IGenericRepository<Seller> _genericRepository;
    private readonly IPasswordHasher<Seller> _passwordHasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public SellerCreateCommandHandler(IGenericRepository<Seller> genericRepository, IJwtTokenGenerator tokenGenerator,
        IPasswordHasher<Seller> passwordHasher, ICloudUploaderService cloudUploaderService)
    {
        _genericRepository = genericRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
        _cloudUploaderService = cloudUploaderService;
    }

    public async ValueTask<Result<AuthResult>> HandleAsync(SellerCreate request, CancellationToken cancellationToken)
    {
        var isExist = await _genericRepository.ExistsAsync(c => c.Username == request.Username);
        if (isExist)
            return Result.Failure<AuthResult>(new Error("400",
                $"Seller with username:'{request.Username}' is already exists"));

        var seller = new Seller(Guid.NewGuid(), request.PhoneNumber, request.Email, request.Title,
            request.Description, request.Info, request.Username, request.FirstName,
            request.LastName, null,
            await _cloudUploaderService.Upload(request.Banner, request.Banner.FileName),
            await _cloudUploaderService.Upload(request.Avatar, request.Avatar.FileName));
        seller.SetPassword(request.Password, _passwordHasher);
        await _genericRepository.AddAsync(seller);
        return Result.Success(new AuthResult(new Authorized(seller.Id, seller.FirstName,
                seller.LastName, seller.PhoneNumber, seller.Email, Roles.Seller.ToString()),
            _tokenGenerator.GenerateToken(seller.ToTokenRequest())));
    }
}