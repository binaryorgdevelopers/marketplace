using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Messages;

public class BadgeDto : BaseDto<BadgeDto, Badge>
{
    public BadgeDto(Guid i, string tex, string textColo, string backgroundColo, string description, string type)
    {
        I = i;
        Tex = tex;
        TextColo = textColo;
        BackgroundColo = backgroundColo;
        Description = description;
        Type = type;
    }

    public BadgeDto()
    {
        
    }

    public Guid I { get; set; }
    public string Tex { get; set; }
    public string TextColo { get; set; }
    public string BackgroundColo { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
}