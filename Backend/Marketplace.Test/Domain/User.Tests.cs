using Authentication;
using Authentication.Enum;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Marketplace.Test.Domain;

public class UserTests
{
    [Fact]
    public void ValidatePassword_WithValidPassword_ReturnsTrue()
    {
        // Arrange
        var user = new User
        {
            Id = default,
            CreatedAt = default,
            UpdatedAt = default,
            LastSession = default,
            Role = Roles.Admin,
            FirstName = null,
            LastName = null,
            PhoneNumber = null,
            Email = null,
            Files = null,
            ShopId = 0
        };
        var password = "validPassword";
        var passwordHasherMock = new Mock<IPasswordHasher<User>>();
        user.SetPassword(password, passwordHasherMock.Object);
        
        passwordHasherMock.Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, password))
            .Returns(PasswordVerificationResult.Success);

        // Act
        var result = user.ValidatePassword(password, passwordHasherMock.Object);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("998997541642")]
    [InlineData("+i98997541643")]
    [InlineData("+998997iuyt41644")]
    [InlineData("99v8997541645")]
    [InlineData("998f9 9541672")]
    [InlineData("+99899 7541692")]
    public void ValidateWrongPhoneNumber(string phoneNumber)
    {
        // Assert.False(User.PhoneNumberValidate(phoneNumber));
    }

    [Fact]
    public void ItShouldThrowExceptionOnInvalidRole()
    {
        Assert.Throws<AuthException>(() => new User(Guid.Empty, "khamidovdilshodbek@gmail.com", ""));
    }

    [Fact]
    public void ItShouldSetDefaultRoleIfEmpty()
    {
        var user = new User(Guid.Empty, "khamidovdilshodbek@gmail.com", "");
        Assert.Equal("user", Enum.GetName(typeof(Roles), user.Role));
    }
}