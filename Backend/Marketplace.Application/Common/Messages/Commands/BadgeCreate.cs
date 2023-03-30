using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Commands;

public record BadgeCreate(
    string Text,
    string TextColor,
    string BackgroundColor,
    string Description,
    string? Link
)
{
    public Badge ToBadge() => Badge.Create(Text, TextColor, BackgroundColor, Description);
};