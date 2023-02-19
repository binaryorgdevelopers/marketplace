using FluentValidation;
using Marketplace.Application.Common.Messages.Commands;
using Marketplace.Domain.Constants;

namespace Marketplace.Api.Validations;

public class SignUpValidation : AbstractValidator<SignUp>
{
    public SignUpValidation()
    {
        RuleFor(c => c.Email).EmailAddress();
        RuleFor(c => c.Password).MinimumLength(8);
        RuleFor(c => c.PhoneNumber)
            .Matches(Regexs.PhoneNumberRegexPatter);
    }
}