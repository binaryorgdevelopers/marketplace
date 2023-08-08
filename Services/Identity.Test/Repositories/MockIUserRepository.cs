using EventBus.Models;
using Identity.Domain.Entities;
using Identity.Infrastructure.Repositories;
using Identity.Models;
using Moq;

namespace Identity.Test.Repositories;

internal class MockIUserRepository
{
    public static Mock<IUserRepository> GetMock()
    {
        var mock = new Mock<IUserRepository>();

        var users = new List<User>()
        {
            new("Dilshodbek", "Hamidov", "khamidovdilshodbek@gmail.com", "+998997541642", Guid.NewGuid(), "UZ"),
            new("Dilshodbek", "Hamidov", "dilshodbekkhamidov@gmail.com", "+998997541642", Guid.NewGuid(), "UZ"),
            new("Dilshodbek", "Hamidov", "dilshod@gmail.com", "+998997541642", Guid.NewGuid(), "UZ")
        };

        mock
            .Setup(m => m.GetUserByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns((string email) =>
            {
                var user = users.FirstOrDefault(u => u.Email.Equals(email));
                if (user is null) return new UserDto();
                var userDto = new UserDto(user.Id, user.FirstName, user.LastName, user.Email, "ADMIN");
                return userDto;

            });

        mock
            .Setup(m => m.GetUserById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns((Guid id) =>
            {
                var user = users.FirstOrDefault(u => u.Id.Equals(id));
                if (user is null) return new UserDto();
                var userDto = new UserDto(user.Id, user.FirstName, user.LastName, user.Email, "ADMIN");
                return userDto;

            });

        mock
            .Setup(m => m.AddAsync(It.IsAny<UserCreateCommand>(), It.IsAny<CancellationToken>()))
            .Returns((UserCreateCommand command) =>
            {
                var user = new User(command.Firstname, command.Lastname, command.Email, command.PhoneNumber,
                    command.RoleId.Value);
                users.Add(user);
                return user;
            });


        return mock;
    }
}