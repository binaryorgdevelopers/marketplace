using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Events;
using Inventory.Domain.Abstractions.Repositories;
using Inventory.Domain.Abstractions.Services;
using Inventory.Domain.Entities;
using Inventory.Domain.Models.Constants;
using Inventory.Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Application.Commands.Command.Authentication;

public class SellerCreateCommandHandler : ICommandHandler<SellerCreate, AuthResult>
{
    private readonly IGenericRepository<Seller> _genericRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHasher<Seller> _passwordHasher;
    private readonly ICloudUploaderService _cloudUploaderService;

    public SellerCreateCommandHandler(IGenericRepository<Seller> genericRepository, IJwtTokenGenerator tokenGenerator,
        IPasswordHasher<Seller> passwordHasher, ICloudUploaderService cloudUploaderService)
    {
        _genericRepository = genericRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
        _cloudUploaderService = cloudUploaderService;
    }

    public async Task<Result<AuthResult>> Handle(SellerCreate request, CancellationToken cancellationToken)
    {
        var isExist = await _genericRepository.ExistsAsync(c => c.Username == request.Username);
        if (isExist)
            return Result.Failure<AuthResult>(new Error("400",
                $"Seller with username:'{request.Username}' is already exists"));

        Seller seller = new Seller(Guid.NewGuid(), request.PhoneNumber, request.Email, request.Title,
            request.Description, request.Info, request.Username, request.FirstName,
            request.LastName, null,
            await _cloudUploaderService.Upload(request.Banner, request.Banner.FileName),
            await _cloudUploaderService.Upload(request.Avatar, request.Avatar.FileName));
        seller.SetPassword(request.Password, _passwordHasher);
        await _genericRepository.AddAsync(seller);
        return Result.Success(new AuthResult(new Authorized(seller.Id, seller.FirstName,
                seller.LastName, seller.PhoneNumber, seller.Email, Roles.Seller),
            _tokenGenerator.GenerateToken(seller.ToTokenRequest())));
    }
}