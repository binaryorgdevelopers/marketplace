using Inventory.Domain.Entities;

namespace Marketplace.Application.Common.Messages.Messages;

public class BlobDto : BaseDto<BlobDto, Blob>
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string? Extras { get; set; }

    public BlobDto(Guid id, string fileName, string? extras)
    {
        Id = id;
        FileName = fileName;
        Extras = extras;
    }

    public BlobDto()
    {
        
    }
}