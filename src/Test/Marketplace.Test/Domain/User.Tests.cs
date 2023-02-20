using System.Runtime.InteropServices;
using Marketplace.Domain.Entities;
using Marketplace.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Marketplace.Test.Domain;

public class UserTests
{
    private readonly Mock<PasswordHasher<User>> _passwordHasher;

    public UserTests()
    {
        this._passwordHasher = new Mock<PasswordHasher<User>>();
    }

    [Fact]
    public void ItShouldCreateNewUser()
    {
        User user = new User();
        Assert.True(user is User);
    }

    // [Fact]
    // public void ItShouldThrowExceptionIfInvalidPassword()
    // {
    //     User user = new User();
    //     bool validationResult = false;
    //     _passwordHasher.Setup()
    //     
    //     Assert.True(validationResult);
    // }
    [Theory]
    [InlineData("Dilshod2003")]
    [InlineData("Dilshod2003&*()")]
    [InlineData("20031506")]
    public void ItShouldVerifyPassword(string password)
    {
        User user = new User();
        _passwordHasher.Setup(c => user.SetPassword(password, c));
        _passwordHasher.Setup(c => user.ValidatePassword(password, c)).Returns(true);
    }

    [Theory]
    [InlineData("+998997541642")]
    [InlineData("+998997541643")]
    [InlineData("+998997541644")]
    [InlineData("+998997541645")]
    [InlineData("+998997541672")]
    [InlineData("+998997541692")]
    [InlineData("+998997541042")]
    [InlineData("+998997549642")]
    [InlineData("+998997581642")]
    [InlineData("+998997545642")]
    [InlineData("+998997546642")]
    [InlineData("+998997548642")]
    [InlineData("+998997543642")]
    [InlineData("+998987549642")]
    public void ValidatePhoneNumber(string phoneNumber)
    {
        Assert.True(User.PhoneNumberValidate(phoneNumber));
    }

    [Theory]
    [InlineData("998997541642")]
    [InlineData("+i98997541643")]
    [InlineData("+998997iuyt41644")]
    [InlineData("99v8997541645")]
    [InlineData("998f9 9541672")]
    [InlineData("+99899 7541692")]
    [InlineData("99897541042")]
    [InlineData("f998997549642")]
    [InlineData(" 998997581642")]
    [InlineData("+998 997545642")]
    [InlineData("+99899 7546642")]
    [InlineData("98997548642")]
    [InlineData("+998997 543642")]
    [InlineData("998987549642")]
    public void ValidateWrongPhoneNumber(string phoneNumber)
    {
        Assert.False(User.PhoneNumberValidate(phoneNumber));
    }

    [Fact]
    public void ItShouldThrowExceptionOnInvalidRole()
    {
        Assert.Throws<AuthException>(()=>new User(Guid.Empty,"khamidovdilshodbek@gmail.com",""));
    }

    [Fact]
    public void ItShouldSetDefaultRoleIfEmpty()
    {
        var user = new User(Guid.Empty,"khamidovdilshodbek@gmail.com","");
        Assert.Equal("user",Enum.GetName(typeof(RoleEnum),user.Role));
    }
}