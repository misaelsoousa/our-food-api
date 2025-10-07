using Microsoft.AspNetCore.Http;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;

namespace OurFood.Api.UseCases.Restaurante;

public interface IUpdateRestauranteImagemUseCase
{
    Task<(bool success, string? error, string? imageUrl)> Execute(int id, IFormFile imagemFile);
}

public class UpdateRestauranteImagemUseCase(OurFoodDbContext db, IS3Service s3Service) : IUpdateRestauranteImagemUseCase
{
    public async Task<(bool success, string? error, string? imageUrl)> Execute(int id, IFormFile imagemFile)
    {
        try
        {
            // Buscar o restaurante existente
            var restaurante = db.Restaurantes.FirstOrDefault(r => r.Id == id);
            if (restaurante == null)
                return (false, "Restaurante não encontrado", null);

            // Fazer upload da nova imagem
            var caminhoImagem = await s3Service.UploadFileAsync(imagemFile, "restaurantes");
            if (string.IsNullOrEmpty(caminhoImagem))
                return (false, "Erro ao fazer upload da imagem", null);

            // Atualizar a imagem no banco
            restaurante.Imagem = caminhoImagem;
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
