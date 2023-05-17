using Inventory.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Messages;

public class CategoryDto : BaseDto<CategoryDto, Category>
{
    public CategoryDto(Guid id, string title, int productAmount, IEnumerable<ProductDto> products)
    {
        Id = id;
        Title = title;
        ProductAmount = productAmount;
        // Products = products;
    }

    public CategoryDto(Guid id, string title, int productAmount, IEnumerable<ProductDto> products, CategoryDto? parent)
    {
        Id = id;
        Title = title;
        ProductAmount = productAmount;
        // Products = products;
        Parent = parent;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public int ProductAmount { get; set; }
    // public IEnumerable<ProductDto> Products { get; set; }
    public CategoryDto? Parent { get; set; }
    // public Guid? ParentId { get; set; }

    public CategoryDto()
    {
    }
}