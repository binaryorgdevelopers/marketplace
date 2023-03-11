using Marketplace.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Messages;

public class CategoryDto : BaseDto<CategoryDto, Category>
{
    public CategoryDto(Guid id, string title, int productAmount, IEnumerable<ProductDto> products)
    {
        Id = id;
        Title = title;
        ProductAmount = productAmount;
        this.products = products;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public int ProductAmount { get; set; }
    public IEnumerable<ProductDto> products { get; set; }

    public CategoryDto()
    {
    }
}