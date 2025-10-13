using Microsoft.AspNetCore.Http;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;

namespace OurFood.Api.UseCases.Categoria;

public interface IUpdateCategoriaImagemUseCase
{
    Task<(bool success, string? error, string? imageUrl)> Execute(int id, IFormFile imagemFile);
}

public class UpdateCategoriaImagemUseCase(OurFoodDbContext db, IS3Service s3Service) : IUpdateCategoriaImagemUseCase
{
    public async Task<(bool success, string? error, string? imageUrl)> Execute(int id, IFormFile imagemFile)
    {
        try
        {
            // Buscar a categoria existente
            var categoria = db.Categorias.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
                return (false, "Categoria não encontrada", null);

            // Fazer upload da nova imagem
            var caminhoImagem = await s3Service.UploadFileAsync(imagemFile, "categorias");
            if (string.IsNullOrEmpty(caminhoImagem))
                return (false, "Erro ao fazer upload da imagem", null);

            // Atualizar a imagem no banco
            categoria.Imagem = caminhoImagem;
            db.SaveChanges();

            // Retornar a URL completa da nova imagem
            var imageUrl = s3Service.GetFileUrl(caminhoImagem);
            return (true, null, imageUrl);
        }
        catch (Exception ex)
        {
            return (false, $"Erro interno: {ex.Message}", null);
        }
    }
}

