using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using System;
using System.IO;

namespace OurFood.Api.Commands;

public class MigrateImagesCommand
{
    private readonly OurFoodDbContext _dbContext;
    private readonly IS3Service _s3Service;

    public MigrateImagesCommand(OurFoodDbContext dbContext, IS3Service s3Service)
    {
        _dbContext = dbContext;
        _s3Service = s3Service;
    }

    public async Task ExecuteAsync()
    {
        Console.WriteLine("=== Iniciando migração de imagens para S3 ===");

        // Migrar imagens de produtos
        await MigrateProdutoImagesAsync();

        // Migrar imagens de categorias
        await MigrateCategoriaImagesAsync();

        // Migrar imagens de restaurantes
        await MigrateRestauranteImagesAsync();

        Console.WriteLine("=== Migração concluída ===");
    }

    private async Task MigrateProdutoImagesAsync()
    {
        Console.WriteLine("Migrando imagens de produtos...");

        var produtos = await _dbContext.Produtos
            .Where(p => !string.IsNullOrEmpty(p.Imagem) && !p.Imagem.StartsWith("https://"))
            .ToListAsync();

        foreach (var produto in produtos)
        {
            try
            {
                var localPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", produto.Imagem);
                
                if (File.Exists(localPath))
                {
                    Console.WriteLine($"Migrando: {produto.Imagem}");
                    
                    // Ler arquivo local
                    var fileBytes = await File.ReadAllBytesAsync(localPath);
                    var fileName = Path.GetFileName(produto.Imagem);
                    var contentType = GetContentType(fileName);
                    
                    // Criar IFormFile simulado
                    var formFile = new SimulatedFormFile(fileBytes, fileName, contentType);
                    
                    // Upload para S3
                    var s3Key = await _s3Service.UploadFileAsync(formFile, "produtos");
                    
                    // Atualizar banco de dados
                    produto.Imagem = s3Key;
                    _dbContext.Produtos.Update(produto);
                    
                    Console.WriteLine($"✓ Migrado: {produto.Imagem} -> {s3Key}");
                }
                else
                {
                    Console.WriteLine($"⚠ Arquivo não encontrado: {localPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Erro ao migrar {produto.Imagem}: {ex.Message}");
            }
        }

        await _dbContext.SaveChangesAsync();
        Console.WriteLine($"Produtos migrados: {produtos.Count}");
    }

    private async Task MigrateCategoriaImagesAsync()
    {
        Console.WriteLine("Migrando imagens de categorias...");

        var categorias = await _dbContext.Categorias
            .Where(c => !string.IsNullOrEmpty(c.Imagem) && !c.Imagem.StartsWith("https://"))
            .ToListAsync();

        foreach (var categoria in categorias)
        {
            try
            {
                var localPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", categoria.Imagem);
                
                if (File.Exists(localPath))
                {
                    Console.WriteLine($"Migrando: {categoria.Imagem}");
                    
                    var fileBytes = await File.ReadAllBytesAsync(localPath);
                    var fileName = Path.GetFileName(categoria.Imagem);
                    var contentType = GetContentType(fileName);
                    
                    var formFile = new SimulatedFormFile(fileBytes, fileName, contentType);
                    var s3Key = await _s3Service.UploadFileAsync(formFile, "categorias");
                    
                    categoria.Imagem = s3Key;
                    _dbContext.Categorias.Update(categoria);
                    
                    Console.WriteLine($"✓ Migrado: {categoria.Imagem} -> {s3Key}");
                }
                else
                {
                    Console.WriteLine($"⚠ Arquivo não encontrado: {localPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Erro ao migrar {categoria.Imagem}: {ex.Message}");
            }
        }

        await _dbContext.SaveChangesAsync();
        Console.WriteLine($"Categorias migradas: {categorias.Count}");
    }

    private async Task MigrateRestauranteImagesAsync()
    {
        Console.WriteLine("Migrando imagens de restaurantes...");

        var restaurantes = await _dbContext.Restaurantes
            .Where(r => !string.IsNullOrEmpty(r.Imagem) && !r.Imagem.StartsWith("https://"))
            .ToListAsync();

        foreach (var restaurante in restaurantes)
        {
            try
            {
                var localPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", restaurante.Imagem);
                
                if (File.Exists(localPath))
                {
                    Console.WriteLine($"Migrando: {restaurante.Imagem}");
                    
                    var fileBytes = await File.ReadAllBytesAsync(localPath);
                    var fileName = Path.GetFileName(restaurante.Imagem);
                    var contentType = GetContentType(fileName);
                    
                    var formFile = new SimulatedFormFile(fileBytes, fileName, contentType);
                    var s3Key = await _s3Service.UploadFileAsync(formFile, "restaurantes");
                    
                    restaurante.Imagem = s3Key;
                    _dbContext.Restaurantes.Update(restaurante);
                    
                    Console.WriteLine($"✓ Migrado: {restaurante.Imagem} -> {s3Key}");
                }
                else
                {
                    Console.WriteLine($"⚠ Arquivo não encontrado: {localPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Erro ao migrar {restaurante.Imagem}: {ex.Message}");
            }
        }

        await _dbContext.SaveChangesAsync();
        Console.WriteLine($"Restaurantes migrados: {restaurantes.Count}");
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }
}

public class SimulatedFormFile : Microsoft.AspNetCore.Http.IFormFile
{
    private readonly byte[] _fileBytes;
    private readonly string _fileName;
    private readonly string _contentType;

    public SimulatedFormFile(byte[] fileBytes, string fileName, string contentType)
    {
        _fileBytes = fileBytes;
        _fileName = fileName;
        _contentType = contentType;
    }

    public string ContentType => _contentType;
    public string ContentDisposition => $"form-data; name=\"file\"; filename=\"{_fileName}\"";
    public Microsoft.AspNetCore.Http.IHeaderDictionary Headers => new Microsoft.AspNetCore.Http.HeaderDictionary();
    public long Length => _fileBytes.Length;
    public string Name => "file";
    public string FileName => _fileName;

    public Stream OpenReadStream()
    {
        return new MemoryStream(_fileBytes);
    }

    public void CopyTo(Stream target)
    {
        target.Write(_fileBytes, 0, _fileBytes.Length);
    }

    public Task CopyToAsync(Stream target, System.Threading.CancellationToken cancellationToken = default)
    {
        return target.WriteAsync(_fileBytes, 0, _fileBytes.Length, cancellationToken);
    }
}