using Microsoft.AspNetCore.Http;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Produto;

public interface IUpdateProdutoImagemUseCase
{
    Task<(bool success, string? error, string? imageUrl)> Execute(int id, IFormFile imagemFile);
}

public class UpdateProdutoImagemUseCase(OurFoodDbContext db, IS3Service s3Service) : IUpdateProdutoImagemUseCase
{
    public async Task<(bool success, string? error, string? imageUrl)> Execute(int id, IFormFile imagemFile)
    {
        try
        {
            // Buscar o produto existente
            var produto = db.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
                return (false, "Produto não encontrado", null);

            // Fazer upload da nova imagem
            var caminhoImagem = await s3Service.UploadFileAsync(imagemFile, "produtos");
            if (string.IsNullOrEmpty(caminhoImagem))
                return (false, "Erro ao fazer upload da imagem", null);

            // Atualizar a imagem no banco
            produto.Imagem = caminhoImagem;
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
