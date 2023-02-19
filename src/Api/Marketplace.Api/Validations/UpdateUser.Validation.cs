using FluentValidation;
using Marketplace.Application.Common.Messages.Commands;

namespace Marketplace.Api.Validations;

public class UpdateUserValidation:AbstractValidator<UpdateUser>
{
    public UpdateUserValidation()
    {
        RuleFor(c=>c.Email).EmailAddress();
        RuleFor(c=>c.Password).MinimumLength(8);
    }
}