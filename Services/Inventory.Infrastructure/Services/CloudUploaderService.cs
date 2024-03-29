﻿using System.Net.NetworkInformation;
using Amazon.S3;
using Amazon.S3.Model;
using Inventory.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Models;

namespace Marketplace.Infrastructure.Services;

public class CloudUploaderService : ICloudUploaderService
{
    private readonly AWSCredentials _options;
    private readonly IAmazonS3 _s3;
    private readonly ILogger<CloudUploaderService> _logger;


    public CloudUploaderService( IOptions<AWSCredentials> options, IAmazonS3 s3,ILogger<CloudUploaderService> logger)
    {
        _s3 = s3;
        _logger = logger;
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