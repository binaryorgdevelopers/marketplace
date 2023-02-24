using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Commands;

public record CharacteristicsCreate(string Title, IEnumerable<ColorCreate> Colors)
{
    public readonly string Title = Title;
    public readonly IEnumerable<ColorCreate> Colors = Colors;

    public Characteristics MapToChars()
    {
        return new Characteristics(Colors.Select(c => c.ToColor()), Title);
    }
}