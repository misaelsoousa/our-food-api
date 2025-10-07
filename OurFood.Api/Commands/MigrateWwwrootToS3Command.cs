using Microsoft.Extensions.DependencyInjection;
using OurFood.Api.Services;
using OurFood.Api.Infrastructure;

namespace OurFood.Api.Commands;

public class MigrateWwwrootToS3Command
{
    private readonly OurFoodDbContext _dbContext;
    private readonly IS3Service _s3Service;

    public MigrateWwwrootToS3Command(OurFoodDbContext dbContext, IS3Service s3Service)
    {
        _dbContext = dbContext;
        _s3Service = s3Service;
    }

    public async Task ExecuteAsync()
    {
        Console.WriteLine("=== Iniciando migração de imagens do wwwroot para S3 ===");

        // Migrar categorias
        await MigrateCategoriasAsync();
        
        // Migrar produtos
        await MigrateProdutosAsync();
        
        // Migrar restaurantes
        await MigrateRestaurantesAsync();

        Console.WriteLine("=== Migração concluída! ===");
    }

    private async Task MigrateCategoriasAsync()
    {
        Console.WriteLine("Migrando imagens de categorias...");
        
        var categorias = _dbContext.Categorias.ToList();
        
        foreach (var categoria in categorias)
        {
            if (!string.IsNullOrEmpty(categoria.Imagem) && 
                categoria.Imagem.StartsWith("imagens/categorias/") &&
                !categoria.Imagem.StartsWith("https://"))
            {
                var localPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", categoria.Imagem);
                
                if (File.Exists(localPath))
                {
                    try
                    {
                        // Criar um IFormFile temporário
                        var fileBytes = await File.ReadAllBytesAsync(localPath);
                        var fileName = Path.GetFileName(categoria.Imagem);
                        
                        using var stream = new MemoryStream(fileBytes);
                        var formFile = new FormFile(stream, 0, fileBytes.Length, "file", fileName)
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = GetContentType(fileName)
                        };

                        // Upload para S3
                        var s3Key = await _s3Service.UploadFileAsync(formFile, "categorias");
                        
                        // Atualizar no banco
                        categoria.Imagem = s3Key;
                        _dbContext.SaveChanges();
                        
                        Console.WriteLine($"✅ Categoria {categoria.Nome}: {categoria.Imagem} -> {s3Key}");
                        
                        // Opcional: deletar arquivo local
                        // File.Delete(localPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Erro ao migrar categoria {categoria.Nome}: {ex.Message}");
                    }
                }
            }
        }
    }

    private async Task MigrateProdutosAsync()
    {
        Console.WriteLine("Migrando imagens de produtos...");
        
        var produtos = _dbContext.Produtos.ToList();
        
        foreach (var produto in produtos)
        {
            if (!string.IsNullOrEmpty(produto.Imagem) && 
                produto.Imagem.StartsWith("imagens/produtos/") &&
                !produto.Imagem.StartsWith("https://"))
            {
                var localPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", produto.Imagem);
                
                if (File.Exists(localPath))
                {
                    try
                    {
                        var fileBytes = await File.ReadAllBytesAsync(localPath);
                        var fileName = Path.GetFileName(produto.Imagem);
                        
                        using var stream = new MemoryStream(fileBytes);
                        var formFile = new FormFile(stream, 0, fileBytes.Length, "file", fileName)
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = GetContentType(fileName)
                        };

                        var s3Key = await _s3Service.UploadFileAsync(formFile, "produtos");
                        produto.Imagem = s3Key;
                        _dbContext.SaveChanges();
                        
                        Console.WriteLine($"✅ Produto {produto.Nome}: {produto.Imagem} -> {s3Key}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Erro ao migrar produto {produto.Nome}: {ex.Message}");
                    }
                }
            }
        }
    }

    private async Task MigrateRestaurantesAsync()
    {
        Console.WriteLine("Migrando imagens de restaurantes...");
        
        var restaurantes = _dbContext.Restaurantes.ToList();
        
        foreach (var restaurante in restaurantes)
        {
            if (!string.IsNullOrEmpty(restaurante.Imagem) && 
                restaurante.Imagem.StartsWith("imagens/restaurantes/") &&
                !restaurante.Imagem.StartsWith("https://"))
            {
                var localPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", restaurante.Imagem);
                
                if (File.Exists(localPath))
                {
                    try
                    {
                        var fileBytes = await File.ReadAllBytesAsync(localPath);
                        var fileName = Path.GetFileName(restaurante.Imagem);
                        
                        using var stream = new MemoryStream(fileBytes);
                        var formFile = new FormFile(stream, 0, fileBytes.Length, "file", fileName)
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = GetContentType(fileName)
                        };

                        var s3Key = await _s3Service.UploadFileAsync(formFile, "restaurantes");
                        restaurante.Imagem = s3Key;
                        _dbContext.SaveChanges();
                        
                        Console.WriteLine($"✅ Restaurante {restaurante.Nome}: {restaurante.Imagem} -> {s3Key}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Erro ao migrar restaurante {restaurante.Nome}: {ex.Message}");
                    }
                }
            }
        }
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
