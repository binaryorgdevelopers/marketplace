using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Messages;

public class ProductDto : BaseDto<ProductDto, Product>
{
    public ProductDto(Guid id, string[]? attributes, IEnumerable<BadgeDto> badges, string? synonyms, string title,
        string description, CategoryDto category, SellerDto seller, IEnumerable<BlobDto> photos,
        IEnumerable<CharacteristicsDto> characteristics)
    {
        Id = id;
        Attributes = attributes;
        Badges = badges;
        Synonyms = synonyms;
        Title = title;
        Description = description;
        Category = category;
        Seller = seller;
        Photos = photos;
        Characteristics = characteristics;
    }

    public ProductDto(Guid id, string[]? attributes, IEnumerable<Badge> badges, string? synonyms, string title,
        string description, CategoryDto category, SellerDto seller, IEnumerable<Blob> photos,
        IEnumerable<Characteristics> characteristics)
    {
        Id = id;
        Attributes = attributes;
        Badges = badges.Select(c => new BadgeDto(c.Id, c.Text, c.TextColor, c.BackgroundColor, c.Description, c.Type));
        Synonyms = synonyms;
        Title = title;
        Description = description;
        Category = category;
        Seller = seller;
        Photos = photos.Select(BlobDto.FromEntity);
        Characteristics = characteristics.Select(CharacteristicsDto.FromEntity);
    }

    public ProductDto()
    {
    }

    public Guid Id { get; set; }
    public string[]? Attributes { get; set; }
    public IEnumerable<BadgeDto> Badges { get; set; }
    public string? Synonyms { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public CategoryDto Category { get; set; }
    public SellerDto Seller { get; set; }
    public IEnumerable<BlobDto> Photos { get; set; }
    public IEnumerable<CharacteristicsDto> Characteristics { get; set; }
}