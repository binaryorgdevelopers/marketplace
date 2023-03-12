﻿using Marketplace.Domain.Shared;
using MediatR;

namespace Marketplace.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}