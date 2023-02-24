namespace Marketplace.Application.Common.Messages.Messages;

public record CategoryRead(
    Guid Id,
    string Title,
    int ProductAmount,
    IEnumerable<ProductRead> products
);