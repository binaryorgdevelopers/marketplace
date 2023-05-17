using Microsoft.AspNetCore.Http;

namespace Inventory.Domain.Abstractions.Services;

public interface ICloudUploaderService
{
    Task<string> Upload(IFormFile file, string? fileName);
    Task<string> Upload(string file, string? fileName);
}