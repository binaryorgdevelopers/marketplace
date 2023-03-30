using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Commands;

public sealed record ColorCreate(
    string Title, string Value)
{
    public string Title = Title;
    public string Value = Value;


    public Color ToColor() => new(Title, Value);
}