using System.Linq.Expressions;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using Identity.Infrastructure.Services;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shared.Models;
using Shared.Models.Constants;

namespace Identity.Test.Services;

public class UserManagerServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ITokenProvider> _tokenProviderMock;
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly UserManagerService _userManagerService;

    private readonly User _user;
    private readonly Role _role;

    public UserManagerServiceTest()
    {
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _tokenProviderMock = new Mock<ITokenProvider>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _userManagerService = new UserManagerService(
            repository: _userRepositoryMock.Object,
            tokenProvider: _tokenProviderMock.Object,
            roleRepository: _roleRepositoryMock.Object,
            _passwordHasherMock.Object);

        _user = new User("Dilshodbek", "Hamidov",
            "khamidovdilshodbek@gmail.com", "+998997541642", Guid.NewGuid());

        _role = new Role("Admin");
    }

    [Fact]
    public async Task Should_Return_Invalid_Code_Error_WhenInvalidRole()
    {
        // Arrange
        var command = new UserCreateCommand(
            "+998997541642",
            "khamidovdilshodbek@gmail.com",
            "dilshod2003",
            "Dilshodbek",
            "Hamidov",
            Guid.Parse("57777695-1022-404c-8136-ddaf016f57bd"));


        _userRepositoryMock.Setup(
                x => x.GetUserByEmail(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(_user);

        // Act
        Result<AuthResult> result = await _userManagerService.Register(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(result.Error.Code, Codes.InvalidRole);
    }

    [Theory]
    [InlineData(
        "+998997541642", "khamidovdilshodbek@gmail.com", "dilshod2003",
        "Dilshodbek", "Hamidov", "57777695-1022-404c-8136-ddaf016f57bd")]
    public async Task Should_Return_Already_Exist_Code_When_DuplicateUser(
        string phoneNumber,
        string email,
        string password,
        string firstName,
        string lastName,
        string roleId)
    {
        // Arrange
        var command = new UserCreateCommand(phoneNumber, email, password, firstName, lastName, Guid.Parse(roleId));

        _userRepositoryMock
            .Setup(c => c.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Result<AuthResult> result = await _userManagerService.Register(command);

        //Assert

        _userRepositoryMock.Verify(
            x => x.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once());
        Assert.Equal(Codes.EmailInUse, result.Error.Code);
        Assert.NotNull(result.Error);
        Assert.Null(result.Value);
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [InlineData("dishka@gmail.com", "Dilshod")]
    [InlineData("adfasd@gmail.com", "Dilshod")]
    [InlineData("5432534@gmail.com", "Dilshod")]
    public async Task Should_Return_UserNotFount_If_UserNotExist_When_Logging_In(
        string email, string password)
    {
        // Arrange
        var command = new UserSignInCommand(email, password);
        _userRepositoryMock.Setup(
                x => x.GetUserByEmail(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User());
        // Act
        Result<AuthResult> result = await _userManagerService.Login(command);
        // Assert
        Assert.Equal(Codes.UserNotFound, result.Error.Code);
        Assert.Null(result.Value);
    }

    [Theory]
    [InlineData("dishka@gmail.com", "Dilshod")]
    [InlineData("adfasd@gmail.com", "Dilshod")]
    [InlineData("5432534@gmail.com", "Dilshod")]
    public async Task Should_Return_InvalidPassword_When_Logging_In(string email, string password)
    {
        _user.SetPassword("adfsad", _passwordHasherMock.Object);
        // Arrange
        var command = new UserSignInCommand(email, password);
        _userRepositoryMock.Setup(
                x => x.GetUserByEmail(It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(_user);
        // Act
        Result<AuthResult> result = await _userManagerService.Login(command);
        // Assert
        Assert.Equal(Codes.InvalidPassword, result.Error.Code);
        Assert.Null(result.Value);
    }
}