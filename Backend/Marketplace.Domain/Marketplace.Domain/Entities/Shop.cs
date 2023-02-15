using Marketplace.Domain.Abstractions;

namespace Marketplace.Domain.Entities;

public class Shop:IIdentifiable
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public string Extras { get; set; }
}