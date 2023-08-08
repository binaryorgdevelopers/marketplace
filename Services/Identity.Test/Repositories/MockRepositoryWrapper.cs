using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.Test.Repositories;

internal class RepositoryWrapper : IRepositoryWrapper
{
    private IdentityContext _context;
    private IUserRepository _user;
    private IRoleRepository _role;
    private IPasswordHasher<User> _passwordHasher;
    private ICacheRepository _cacheRepository;

    public IUserRepository User
    {
        get
        {
            if (_user == null)
            {
                _user = new UserRepository(context: _context, passwordHasher: _passwordHasher,
                    cacheRepository: _cacheRepository);
            }

            return _user;
        }
    }

    public IRoleRepository Role
    {
        get
        {
            if (_role == null)
            {
                _role = new RoleRepository(identityContext: _context);
            }

            return _role;
        }
    }

    public RepositoryWrapper(IdentityContext context, CacheRepository cacheRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _cacheRepository = cacheRepository;
        _passwordHasher = passwordHasher;
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}

internal class MockRepositoryWrapper
{
    public static Mock<IRepositoryWrapper> GetMock()
    {
        var mock = new Mock<IRepositoryWrapper>();

        mock.Setup(m => m.User).Returns(() => new Mock<IUserRepository>().Object);
        mock.Setup(m => m.Role).Returns(() => new Mock<IRoleRepository>().Object);
        mock.Setup(m => m.Save()).Callback(() => { });
        return mock;
    }
}

internal interface IRepositoryWrapper
{
    IUserRepository User { get; }
    IRoleRepository Role { get; }
    void Save();
}