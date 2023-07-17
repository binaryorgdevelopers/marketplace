using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Inventory.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Marketplace.Infrastructure.Services;

public class FileUploaderService : ICloudUploaderService
{
    private readonly List<char> alphabets = new()
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
    };

    private readonly HttpClient _httpClient;

    public FileUploaderService(IHttpClientFactory factory, IOptions<FileServerConfiguration> options)
    {
        _httpClient = factory.CreateClient("fileServer");
    }

    public async Task<string> Upload(IFormFile file, string? fileName)
    {
        using var multipartFormContent = new MultipartContent();
        var fileStreamContent = new StreamContent(file.OpenReadStream());
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

        multipartFormContent.Add(fileStreamContent);

        var response = await _httpClient.PostAsync("file/save", multipartFormContent);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> Upload(string file, string? fileName)
    {
        byte[] buffer = Convert.FromBase64String(file);

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "file/save");

        using var content = new MultipartFormDataContent();

        using var stream = new MemoryStream(buffer);
        string extension = GetFileExtension(buffer);
        content.Add(new StreamContent(stream), "file",
            $"{GetRandomString()}{extension}");

        request.Content = content;

        // fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");


        var response = await _httpClient.PostAsync("file/save", content);
        response.EnsureSuccessStatusCode();
        string res = await response.Content.ReadAsStringAsync();
        var serialized = JsonSerializer.Deserialize<FileSaved>(res);
        return serialized?.path;
    }

    private string GetRandomString() =>
        new(Enumerable.Repeat(alphabets, 10).Select(s => s[Random.Shared.Next(s.Count)]).ToArray());

    private string GetFileExtension(byte[] file)
    {
// Define file signature/magic numbers for the file types you want to identify
        Dictionary<string, string> fileSignatures = new Dictionary<string, string>()
        {
            { ".jpeg", "FFD8FF" },
            { ".png", "89504E47" },
            { ".gif", "47494638" },
            { ".bmp", "424D" },
            // Add more file types and signatures here
        };

// Get the first few bytes of the file
        byte[] signatureBytes = new byte[4];
        Array.Copy(file, signatureBytes, 4);

// Convert the bytes to a hex string
        string signature = BitConverter.ToString(signatureBytes).Replace("-", "");

// Loop through the file signatures to find a match
        foreach (KeyValuePair<string, string> fileSignature in fileSignatures)
        {
            if (signature.StartsWith(fileSignature.Value))
            {
                // A matching file signature was found, so return the corresponding file extension
                string fileExtension = fileSignature.Key;
                return fileExtension;
            }
        }

        return string.Empty;
    }
}

public class FileServerConfiguration
{
    public string Url { get; set; }
    public string? Password { get; set; }
    public string? Username { get; set; }

    public FileServerConfiguration()
    {
    }
}

public class FileSaved
{
    [JsonProperty("path")] public string path { get; set; }
    [JsonProperty("size")] public long size { get; set; }
    [JsonProperty("timeStamp")] public string[] timeStamp { get; set; }
}