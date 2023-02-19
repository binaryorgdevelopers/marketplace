using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Domain.Exceptions;

namespace Marketplace.Application.Commands.ICommand;

public interface IUserUpdateCommand
{
    Task<Either<UserUpdated, AuthException>> UpdateUser(UpdateUser user);
}