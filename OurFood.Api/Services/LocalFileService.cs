using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace OurFood.Api.Services;

public class LocalFileService : IS3Service
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    private readonly string _basePath;

    public LocalFileService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
        _basePath = Path.Combine(_environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "imagens");
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Arquivo inválido");

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var folderPath = Path.Combine(_basePath, folder);
        
        // Cria a pasta se não existir
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        // Retorna o caminho relativo (com barras '/' para URL)
        return Path.Combine("imagens", folder, fileName).Replace("\\", "/");
    }

    public async Task<bool> DeleteFileAsync(string key)
    {
        try
        {
            if (string.IsNullOrEmpty(key))
                return false;

            // Remove o prefixo "imagens/" se existir e normaliza o caminho
            var relativePath = key.Replace("imagens/", "").Replace("imagens\\", "").TrimStart('/');
            var filePath = Path.Combine(_basePath, relativePath);
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            
            return false;
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

        // Retorna URL relativa que funciona com UseStaticFiles
        // O UseStaticFiles já serve arquivos de wwwroot, então /imagens/... funciona diretamente
        return key.StartsWith("/") ? key : $"/{key}";
    }
}

