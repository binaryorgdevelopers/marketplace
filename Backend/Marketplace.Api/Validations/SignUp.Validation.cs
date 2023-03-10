using FluentValidation;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Models.Constants;

namespace Marketplace.Api.Validations;

public class SignUpValidation : AbstractValidator<UserCreateCommand>
{
    public SignUpValidation()
    {
        RuleFor(c => c.Email).EmailAddress();
        RuleFor(c => c.Password).MinimumLength(8);
        RuleFor(c => c.PhoneNumber)
            .Matches(Regexs.PhoneNumberRegexPatter);
    }
}