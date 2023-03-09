using Marketplace.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Application.Common.Messages.Commands;

public record SellerCreate(
    string PhoneNumber,
    string Email,
    string Username,
    string FirstName,
    string LastName,
    string Password,
    string Title,
    string Description,
    string Info,
    IFormFile Avatar,
    IFormFile Banner
) : ICommand<AuthResult>;