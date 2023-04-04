using Inventory.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Commands;

public record CharacteristicsCreate(string Title, IEnumerable<ColorCreate> Colors)
{
    public string Title = Title;
    public IEnumerable<ColorCreate> Colors = Colors;

    public Characteristics ToChars() 
        => new(Colors.Select(c => c.ToColor()).ToList(), Title);
}