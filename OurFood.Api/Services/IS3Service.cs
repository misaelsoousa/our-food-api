using Microsoft.AspNetCore.Http;

namespace OurFood.Api.Services;

public interface IS3Service
{
    Task<string> UploadFileAsync(IFormFile file, string folder);
    Task<bool> DeleteFileAsync(string key);
    string GetFileUrl(string key);
}
