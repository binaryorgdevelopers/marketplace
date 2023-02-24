using System.Net.NetworkInformation;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Infrastructure.Services;

public class CloudUploaderService : ICloudUploaderService
{
    private readonly StorageClient _storageClient;
    private readonly ILoggingBroker _logger;
    private readonly string? _bucketName;


    public CloudUploaderService(ILoggingBroker logger)
    {
        _logger = logger;
        string authPath = Directory.GetCurrentDirectory() + @"\auth.json";
        var googleCredential = GoogleCredential.FromFile(authPath);
        _storageClient = StorageClient.Create(googleCredential);
        _bucketName = "streamingapp";
    }

    public Task<string> Upload(IFormFile file, string? fileName) =>
        TryCatch(async () =>
        {
            fileName = fileName ?? new Random().Next(1, 100000).ToString();
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            _logger.LogInformation($"Request for uploading :{fileName}");
            await _storageClient.UploadObjectAsync(_bucketName, fileName, null, memoryStream);
            _logger.LogInformation($"File uploaded");
            return $"https://storage.cloud.google.com/streamingapp/{fileName}";
        });

    private async Task<T> TryCatch<T>(Func<Task<T>> function)
    {
        try
        {
            return await function();
        }
        catch (NetworkInformationException networkInformationException)
        {
            var error =
                $"Network error :${networkInformationException.Message} , Code:{networkInformationException.ErrorCode}";
            _logger.LogInformation(error);
            throw new Exception(error);
        }
    }
}