namespace Marketplace.Application.Common.Messages.Messages;

public record CharacteristicsRead(
    Guid Id,
    string Title,
    IEnumerable<ColorRead> Values);