using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace OurFood.Api.Services;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _configuration;
    private readonly string _bucketName;

    public S3Service(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _configuration = configuration;
        _bucketName = _configuration["AWS:S3:BucketName"] ?? throw new ArgumentNullException("AWS:S3:BucketName não configurado");
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Arquivo inválido");

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var key = $"{folder}/{fileName}";

        using var stream = file.OpenReadStream();
        
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = stream,
            ContentType = file.ContentType,
            ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
        };

        await _s3Client.PutObjectAsync(putRequest);

        return key;
    }

    public async Task<bool> DeleteFileAsync(string key)
    {
        try
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(deleteRequest);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string GetFileUrl(string key)
    {
        if (string.IsNullOrEmpty(key))
            return string.Empty;

        var region = _configuration["AWS:Region"];
        return $"https://{_bucketName}.s3.{region}.amazonaws.com/{key}";
    }
}
