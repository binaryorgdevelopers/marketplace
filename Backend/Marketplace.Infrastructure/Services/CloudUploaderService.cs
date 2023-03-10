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
        var googleCredential = GoogleCredential.FromJson(
            "{\"type\": \"service_account\",\"project_id\": \"ascendant-pad-367109\", \"private_key_id\": \"6a453f11b392659a5418886774b9901ff03bc397\", \"private_key\": \"-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQD8m64JO/WXdCCp\nWyVqeOpyv8ioW87MzdKTUNwn5UuRlwEPqKpaPHwUvaT9TxdDCnOQdPdBx2RNjFeY\nKNkLccEu8GJGvwAFcPYL4eGNi6G+icMJaQNen5F0wAPq6hy3k9WoJTAtkumQ/KID\nYEzhLVwDklW2dUIRuVaSO+TkBXoTYhQaB/ZeyoU0sab4ZqhlhZa1OJvxw9R2YDDn\nmCfr8a74ML5a49PG5xg5Hvic3rfluSoYDAkl46XeOdQgDsx/OywoJSqpF5qdCryK\n+LKxB3fx0gQNPzN9W8GaJ0kiJ8D9m5gTUjpCuxfydg4uN/DkhSVN78Wx1ELB9nu+\nYhIRY2PhAgMBAAECggEAJNTqM6HQywF9VamFiMMSpCKUi6pp5Rt0Qv3nlnrsttCx\nW0AjfpQSdM+7WSKJaMFYXPgWU+VSbwAx3BdwoNDi8wjNEOBT4iHjWOjZg1LtsCN+\nvZuDM0PJk0qqsjuVGQCZht0iljKKkxT9kR0fIzdYPZO+TaPxOzZZNL8+Pi+86fWj\n16XasLbZuPyFEue5tR8HAOw1h99AyLJkG8voqEIl4ntDq1+BBIw6gV/p4I2ef7Ob\ny5/AKIhHxDnj3fVHThoAtII2Y2pBHjeCxufMkXMNCErKAHKh81MvC83WB4LVOpiC\nR4u+p2K+reBVUYXtw4pedMSxyFbfcAW50M1+3sgHpQKBgQD/ArU3NOlxZD3VtghZ\nxU6XYF6/soVlYlZ+OwS4rlzgR+VR9QLoFfumi0kj51zRCjd4xJvhujkiOhF+OFox\nW/Q+Bf+ka5gqeuyz0+olnKdXzlYG/lRet9b06S1NnCZQM4QPgHOQjx41gkHOnrzg\nQNqe45QXNTqWbBmM91LfJaCbxwKBgQD9lpXv2CdvY1+SOlrBH4VbHdzN1y/XpHpf\n5U79pRPn8/aQt7NiaCaBqmmplPPnBL45w1zVw8twxrMW46W2YeMK/+20dGaf8QU+\nggg71ebORp7IBlqrmtRCRAXtMvxkognUPwPlbD3SUhUajIbzxgRKcir/ZI0/mJWl\n2VJzmC1zFwKBgDXLW3D+73PlIoovsUlGrxKN2M71mOBPQ1Gzn4ZZ+euyHvUQC4Hs\nCT8TyOUTDGhokFUgnIWcpCrNNx16Q8R/0mE3ILuNmvbzIXW58X3nswF53HnzMtcc\nTvMxMUZa91YZ1/TxMu++0S+Wf81XfR6Nb9DuzzGqH+bVfPpYvhmmtV/HAoGBAIUU\ndsE0xZU+KFScDS6WdSyZIlxf1nqrlZEnzOltrWGn9TiDPz+gerzHisX1Jn6RTacQ\nsHJ6WxRnImsbssZg3n7SSpPOFmFPYjookVudaI/OHdvJAeJW0ZepDRcrD6tcIh5h\np5DJ2jO64dpZ7ySVQYE0Iv5+bWX3lGlCAHQrY4anAoGBAOwhvsIrzctjr0sMk1LO\nyPXjSlk2EWmObsm72w26JaJpO8SCU/ay8emubwlw2gXtw0+oXFzjtlpA6C8ejA5U\ntTXBpU2B5VRPnQCzUcFrvJ0MnyVdT4vOkc1n0vscL0xmcUfe1HErP1oPtucWLufX\ngPmfeu7QD7zsjxObyqH4+Yyx\n-----END PRIVATE KEY-----\n\", \"client_email\": \"streamingapp@ascendant-pad-367109.iam.gserviceaccount.com\", \"client_id\": \"114908952120167070900\", \"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\", \"token_uri\": \"https://oauth2.googleapis.com/token\",\"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/streamingapp%40ascendant-pad-367109.iam.gserviceaccount.com\"}");
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