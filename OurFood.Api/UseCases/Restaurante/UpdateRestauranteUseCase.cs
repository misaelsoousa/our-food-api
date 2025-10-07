using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Restaurante;

public interface IUpdateRestauranteUseCase
{
    Task<(ResponseRestaurante? response, string? error)> Execute(int id, RequestUpdateRestaurante request, IFormFile? imagemFile);
}

public class UpdateRestauranteUseCase(OurFoodDbContext db, IS3Service s3Service) : IUpdateRestauranteUseCase
{
    public async Task<(ResponseRestaurante? response, string? error)> Execute(int id, RequestUpdateRestaurante request, IFormFile? imagemFile)
    {
        try
        {
            // Buscar o restaurante existente
            var restaurante = db.Restaurantes.FirstOrDefault(r => r.Id == id);
            if (restaurante == null)
                return (null, "Restaurante não encontrado");

            // Atualizar os dados do restaurante
            restaurante.Nome = request.Nome;

            // Se uma nova imagem foi enviada, fazer upload
            if (imagemFile != null && imagemFile.Length > 0)
            {
                var caminhoImagem = await s3Service.UploadFileAsync(imagemFile, "restaurantes");
                if (string.IsNullOrEmpty(caminhoImagem))
                    return (null, "Erro ao fazer upload da imagem");
                
                restaurante.Imagem = caminhoImagem;
            }

            // Salvar as alterações
            db.SaveChanges();

            // Retornar o restaurante atualizado
            return (new ResponseRestaurante(
                restaurante.Id,
                restaurante.Nome,
                !string.IsNullOrEmpty(restaurante.Imagem) ? s3Service.GetFileUrl(restaurante.Imagem) : restaurante.Imagem
            ), null);
        }
        catch (Exception ex)
        {
            return (null, $"Erro interno: {ex.Message}");
        }
    }
}
