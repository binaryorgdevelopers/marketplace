using Authentication;
using Inventory.Domain.Abstractions;
using Shared.Models.Constants;

namespace Inventory.Domain.Entities;

public class Shop : IIdentifiable
{
    public Shop()
    {
    }

    public Shop(Guid id, string name, int number, string extras)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new AuthException(Codes.InvalidCredential, "Name can't be empty");

        Id = id;
        Name = name;
        Number = number;
        Extras = extras;
    }

    public Shop(string name, int number, string extras, Guid userId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Number = number;
        Extras = extras;
        UserId = userId;
    }

    public Shop(Guid? id, string name, int number)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new AuthException(Codes.InvalidCredential, "Name can't be empty");

        Id = id ?? Guid.NewGuid();

        Name = name;
        Number = number;
    }

    public Guid UserId { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public string Extras { get; set; }

    public Guid FileId { get; set; }


    public List<Blob> Files { get; set; }
    public Guid Id { get; set; }
}