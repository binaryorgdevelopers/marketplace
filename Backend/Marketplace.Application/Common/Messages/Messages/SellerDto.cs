using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Messages;

public class SellerDto : BaseDto<SellerDto, Seller>
{
    public SellerDto(Guid id, string title, string description, string info, string username, string firstName,
        string lastName, string banner, string avatar, string link)
    {
        Id = id;
        Title = title;
        Description = description;
        Info = info;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Banner = banner;
        Avatar = avatar;
        Link = link;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Info { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Banner { get; set; }
    public string Avatar { get; set; }
    public string Link { get; set; }
}