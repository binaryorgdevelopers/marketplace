using Marketplace.Application.Common;
using Marketplace.Application.Common.Messages.Messages;
using Marketplace.Application.Queries.IQuery.CategoryQueries;
using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Abstractions.Repositories;
using Marketplace.Domain.Entities;

namespace Marketplace.Application.Queries.Query.CategoryQueries;

public class CategoryReadQuery : ICategoryReadQuery
{
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly ILoggingBroker _loggingBroker;

    public CategoryReadQuery(IGenericRepository<Category> categoryRepository, ILoggingBroker loggingBroker)
    {
        _categoryRepository = categoryRepository;
        _loggingBroker = loggingBroker;
    }

    public IEnumerable<CategoryRead> AllCategories()
    {
        var categories = _categoryRepository
            .GetWithInclude(
                c => c.Products,
                c =>
                    new CategoryRead(c.Id, c.Title, c.ProductAmount, c.Products.Select(x =>
                        new ProductRead(x.Id, x.Attributes, x.Badges.Select(b =>
                                new BadgeRead(b.Id, b.Text, b.TextColor, b.BackgroundColor, b.Description, b.Type)),
                            x.Synonyms, x.Title, x.Description,
                            new CategoryRead(x.Category.Id, x.Category.Title, x.Category.ProductAmount, null),
                            new SellerRead(x.Seller.Id, x.Seller.Title, x.Seller.Description, x.Seller.Info,
                                x.Seller.Username,
                                x.Seller.FirstName, x.Seller.LastName, x.Seller.Banner, x.Seller.Avatar,
                                x.Seller.Link),
                            x.Photos.Select(a => new BlobRead(a.Id, a.Title, a.Extras)),
                            x.Characteristics.Select(s =>
                                new CharacteristicsRead(s.Id, s.Title,
                                    s.Values.Select(v => new ColorRead(v.Id, v.Title, v.Value))))))));

        return categories;
    }
    
    public IEnumerable<CategoryRead> CategoryWithoutProduct()
    {
        var categories = _categoryRepository.GetAll().Select(c =>
            new CategoryRead(c.Id, c.Title, c.ProductAmount, ArraySegment<ProductRead>.Empty));
        return (categories);
    }

    public CategoryRead? CategoryById(Guid id)
    {
        var category = _categoryRepository
            .GetSingleWithInclude(
                c => c.Products,
                c => new CategoryRead(c.Id, c.Title, c.ProductAmount, c.Products.Select(x => new ProductRead(x.Id,
                    x.Attributes,
                    x.Badges.Select(b =>
                        new BadgeRead(b.Id, b.Text, b.TextColor, b.BackgroundColor, b.Description, b.Type)),
                    x.Synonyms, x.Title, x.Description,
                    new CategoryRead(x.Category.Id, x.Category.Title, x.Category.ProductAmount, null),
                    new SellerRead(x.Seller.Id, x.Seller.Title, x.Seller.Description, x.Seller.Info,
                        x.Seller.Username,
                        x.Seller.FirstName, x.Seller.LastName, x.Seller.Banner, x.Seller.Avatar, x.Seller.Link),
                    x.Photos.Select(a => new BlobRead(a.Id, a.Title, a.Extras)),
                    x.Characteristics.Select(s =>
                        new CharacteristicsRead(s.Id, s.Title,
                            s.Values.Select(v => new ColorRead(v.Id, v.Title, v.Value))))))),
                p => p.Id == id);
        return category;
    }

    private async Task<T> TryCatchAsync<T>(Func<Task<T>> function)
    {
        try
        {
            return await function();
        }
        catch (Exception e)
        {
            var error = $"An error occured while processing you request,{e.Message} ";
            _loggingBroker.LogInformation(error);
            throw new Exception(error);
        }
    }

    private T TryCatch<T>(Func<T> function)
    {
        try
        {
            return function();
        }
        catch (Exception e)
        {
            var error = $"An error occured while processing you request,{e.Message} ";
            _loggingBroker.LogInformation(error);
            throw new Exception(error);
        }
    }
}