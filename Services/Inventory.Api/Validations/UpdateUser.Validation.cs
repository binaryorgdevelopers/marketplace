using FluentValidation;
using Marketplace.Application.Common.Messages.Commands;

namespace Inventory.Api.Validations;

public class UpdateUserValidation:AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidation()
    {
        RuleFor(c=>c.Email).EmailAddress();
        RuleFor(c=>c.Password).MinimumLength(8);
    }
}