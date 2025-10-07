using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Categoria;

public interface IUpdateCategoriaUseCase
{
    Task<(ResponseCategoria? response, string? error)> Execute(int id, RequestUpdateCategoria request, IFormFile? imagemFile);
}

public class UpdateCategoriaUseCase(OurFoodDbContext db, IS3Service s3Service) : IUpdateCategoriaUseCase
{
    public async Task<(ResponseCategoria? response, string? error)> Execute(int id, RequestUpdateCategoria request, IFormFile? imagemFile)
    {
        try
        {
            // Buscar a categoria existente
            var categoria = db.Categorias.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
                return (null, "Categoria não encontrada");

            // Atualizar os dados da categoria
            categoria.Nome = request.Nome;
            categoria.CorHex = request.CorHex;

            // Se uma nova imagem foi enviada, fazer upload
            if (imagemFile != null && imagemFile.Length > 0)
            {
                var caminhoImagem = await s3Service.UploadFileAsync(imagemFile, "categorias");
                if (string.IsNullOrEmpty(caminhoImagem))
                    return (null, "Erro ao fazer upload da imagem");
                
                categoria.Imagem = caminhoImagem;
            }

            // Salvar as alterações
            db.SaveChanges();

            // Retornar a categoria atualizada
            return (new ResponseCategoria(
                categoria.Id,
                categoria.Nome,
                categoria.CorHex,
                !string.IsNullOrEmpty(categoria.Imagem) ? s3Service.GetFileUrl(categoria.Imagem) : categoria.Imagem
            ), null);
        }
        catch (Exception ex)
        {
            return (null, $"Erro interno: {ex.Message}");
        }
    }
}
