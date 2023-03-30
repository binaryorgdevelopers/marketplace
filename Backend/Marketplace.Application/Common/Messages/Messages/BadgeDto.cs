using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Messages;

public class BadgeDto : BaseDto<BadgeDto, Badge>
{
    public BadgeDto(Guid id, string text, string textColor, string backgroundColor, string description, string type)
    {
        Id = id;
        Text = text;
        TextColor = textColor;
        BackgroundColor = backgroundColor;
        Description = description;
        Type = type;
    }

    public BadgeDto()
    {
    }

    public Guid Id { get; set; }
    public string Text { get; set; }
    public string TextColor { get; set; }
    public string BackgroundColor { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
}