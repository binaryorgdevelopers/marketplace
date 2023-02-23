﻿using FluentValidation;
using Marketplace.Application.Common.Messages.Commands;

namespace Marketplace.Api.Validations;

public class SignInValidation : AbstractValidator<SignInCommand>
{
    public SignInValidation()
    {
        RuleFor(c => c.Email).EmailAddress();
        RuleFor(c => c.Password).MinimumLength(8);
    }
}