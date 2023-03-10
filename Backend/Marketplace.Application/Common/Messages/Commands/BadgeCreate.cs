namespace Marketplace.Application.Common.Messages.Commands;

public record BadgeCreate(
    string Text,
    string TextColor,
    string BackgroundColor,
    string Description,
    string? Link
) ;