namespace Marketplace.Application.Common.Messages.Messages;

public record BadgeRead(
    Guid Id,
    string Text,
    string TextColor,
    string BackgroundColor,
    string Description,
    string Type
);