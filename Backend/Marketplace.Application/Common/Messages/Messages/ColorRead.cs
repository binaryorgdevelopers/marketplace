namespace Marketplace.Application.Common.Messages.Messages;

public class ColorRead
{
    public ColorRead(Guid id, string title, string value)
    {
        Id = id;
        Title = title;
        Value = value;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Value { get; set; }
}