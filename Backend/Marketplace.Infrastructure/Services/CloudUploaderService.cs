using System.Net.NetworkInformation;
using Amazon.S3;
using Amazon.S3.Model;
using Marketplace.Domain.Abstractions;
using Marketplace.Domain.Abstractions.Services;
using Marketplace.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Marketplace.Infrastructure.Services;

public class CloudUploaderService : ICloudUploaderService
{
    private readonly ILoggingBroker _logger;
    private readonly AWSCredentials _options;
    private readonly IAmazonS3 _s3;


    public CloudUploaderService(ILoggingBroker logger, IOptions<AWSCredentials> options, IAmazonS3 s3)
    {
        _logger = logger;
        _s3 = s3;
        _options = options.Value;
    }

    public Task<string> Upload(IFormFile file, string? fileName) =>
        TryCatch(async () =>
        {
            fileName ??= new Random().Next(1, 100000).ToString();
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            _logger.LogInformation($"Request for uploading :{fileName}");

            var request = new PutObjectRequest
            {
                BucketName = _options.S3BucketName,
                Key = $"images/{fileName}",
                ContentType = file.ContentType,
                InputStream = file.OpenReadStream(),
                Metadata =
                {
                    ["x-amz-meta-originalname"] = file.FileName,
                    ["x-amz-extension"] = Path.GetExtension(file.FileName)
                }
            };
            var result = await _s3.PutObjectAsync(request);
            _logger.LogInformation($"File uploaded");
            return $"https://bindevsbucket.s3.ap-northeast-1.amazonaws.com/images/{fileName}.jpg";
        });

    public Task<string> Upload(string base64Str, string? fileName)
        => TryCatch(async () =>
        {
            byte[] binary = Convert.FromBase64String(base64Str);
            using MemoryStream memoryStream = new MemoryStream(binary);
            fileName ??= new Random().Next(1, 999000).ToString();
            var request = new PutObjectRequest()
            {
                BucketName = _options.S3BucketName,
                Key = $"images/{fileName}",
                ContentType = "text/plain",
                InputStream = memoryStream,
                Metadata =
                {
                    ["x-amz-meta-originalname"] = fileName
                }
            };
            var result = await _s3.PutObjectAsync(request);
            _logger.LogInformation($"File uploaded");
            return $"https://bindevsbucket.s3.ap-northeast-1.amazonaws.com/images/{fileName}.jpg";
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