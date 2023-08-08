using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Identity.Test.Repositories;

public class UserRepositoryTest
{
    private UserRepository _userRepository;

    public UserRepositoryTest()
    {
        Mock<IPasswordHasher<User>> hasher = new Mock<IPasswordHasher<User>>();
        Mock<ICacheRepository> cacheRepository = new Mock<ICacheRepository>();

        var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>()
            .UseInMemoryDatabase("UserContextWithNullCheckingDisabled");
        IdentityContext identityContext = new IdentityContext(optionsBuilder.Options);
        _userRepository = new UserRepository(context: identityContext, passwordHasher: hasher.Object,
            cacheRepository: cacheRepository.Object);
        _userRepository.AddAsync(new UserCreateCommand("+998997541642", "dilshodbekkhamidov@gmail.com", "dilshod",
            "Dilshodbek", "Hamidov"));
    }

    [Theory]
    [InlineData("+998997541642", "dilshodbekkhamidov@gmail.com", "Dilshodbek", "Dilshodbek", "Hamidov")]
    [InlineData("+998997541642", "khamidovdilshodbek@gmail.com", "Dilshodbek", "Dilshodbek", "Hamidov")]
    [InlineData("+998997541642", "dilshod@gmail.com", "Dilshodbek", "Dilshodbek", "Hamidov")]
    public async Task AddUser_Should_Add_New_User(string phoneNumber, string email, string password, string firstName,
        string lastName)
    {
        var command = new UserCreateCommand(phoneNumber, email, password, firstName, lastName, Guid.NewGuid());
        var user = await _userRepository.AddAsync(command);

        Assert.NotNull(user);
    }
    [Theory]
    [InlineData("khamidovdilshodbek@gmail.com")]
    [InlineData("dilshodbekkhamidov@gmail.com")]
    [InlineData("dilshod@gmail.com")]
    public async Task GetUserByEmail_Should_Return_User(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        Assert.NotNull(user);
    }

}