using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Identity.Test.Repositories;

public class UserRepositoryTest
{
    private readonly UserRepository _userRepository;
    private readonly Mock<IdentityContext> _repositoryMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Mock<ICacheRepository> _cacheRepositoryMock;
    public UserRepositoryTest()
    {
        _repositoryMock = new Mock<IdentityContext>();



        var phMock = new Mock<IPasswordHasher<User>>();
        var cacheMock = new Mock<ICacheRepository>();
        _userRepository = new UserRepository(_repositoryMock.Object, phMock.Object,cacheMock.Object);
        
        _repositoryMock = new Mock<IdentityContext>();

        // Create mock for IPasswordHasher<User>
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        // Set up any necessary behavior for the passwordHasherMock

        // Create mock for ICacheRepository
        _cacheRepositoryMock = new Mock<ICacheRepository>();
        // Set up any necessary behavior for the cacheRepositoryMock

        // Create an instance of UserRepository with the mocked dependencies
        _userRepository = new UserRepository(_repositoryMock.Object, _passwordHasherMock.Object, _cacheRepositoryMock.Object);

    }
}