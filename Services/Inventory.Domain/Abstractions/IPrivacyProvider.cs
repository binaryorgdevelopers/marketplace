using Microsoft.AspNetCore.Identity;
using Shared.Models;

namespace Inventory.Domain.Abstractions;

public interface IPrivacyProvider<TEntity> where TEntity : class
{
    bool ValidatePassword(string password, IPasswordHasher<TEntity> passwordHasher);
    void SetPassword(string password, IPasswordHasher<TEntity> passwordHasher);
    bool PhoneNumberValidate(string input);
    TokenRequest ToTokenRequest();
}