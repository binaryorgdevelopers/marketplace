using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Commands;

public record ColorCreate(string Title, string Value)
{
    public readonly string Title = Title;
    public readonly string Value = Value;


    public Color ToColor() =>
        new Color(this.Title, this.Value);
}