﻿using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Exceptions;

namespace Marketplace.Application.Commands.ICommand.Authentication;

public interface IUserCreateCommand
{
    Task<Either<AuthResult, AuthException>> CreateUser(SignUp user);
}