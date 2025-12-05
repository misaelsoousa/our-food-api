using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Produto;

public interface IUpdateProdutoUseCase
{
    Task<(ResponseProduto? response, string? error)> Execute(int id, RequestUpdateProduto request, IFormFile? imagemFile);
}

public class UpdateProdutoUseCase(OurFoodDbContext db, IS3Service fileService) : IUpdateProdutoUseCase
{
    public async Task<(ResponseProduto? response, string? error)> Execute(int id, RequestUpdateProduto request, IFormFile? imagemFile)
    {
        try
        {
            // Buscar o produto existente
            var produto = db.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
                return (null, "Produto não encontrado");

            // Verificar se a categoria existe
            var categoria = db.Categorias.FirstOrDefault(c => c.Id == request.CategoriaId);
            if (categoria == null)
                return (null, "Categoria não encontrada");

            // Verificar se o restaurante existe
            var restaurante = db.Restaurantes.FirstOrDefault(r => r.Id == request.RestauranteId);
            if (restaurante == null)
                return (null, "Restaurante não encontrado");

            // Atualizar os dados do produto
            produto.Nome = request.Nome;
            produto.CategoriaId = request.CategoriaId;
            produto.Preco = request.Preco;
            produto.Descricao = request.Descricao;
            produto.RestauranteId = request.RestauranteId;

            // Se uma nova imagem foi enviada, fazer upload
            if (imagemFile != null && imagemFile.Length > 0)
            {
                var caminhoImagem = await fileService.UploadFileAsync(imagemFile, "produtos");
                if (string.IsNullOrEmpty(caminhoImagem))
                    return (null, "Erro ao fazer upload da imagem");
                
                produto.Imagem = caminhoImagem;
            }

            // Salvar as alterações
            db.SaveChanges();

            // Retornar o produto atualizado com URL completa
            return (new ResponseProduto(
                produto.Id,
                produto.Nome,
                !string.IsNullOrEmpty(produto.Imagem) ? fileService.GetFileUrl(produto.Imagem) : produto.Imagem,
                produto.Preco,
                categoria.Id,
                categoria.Nome ?? string.Empty,
                produto.Descricao ?? string.Empty,
                restaurante.Id,
                restaurante.Nome
            ), null);
        }
        catch (Exception ex)
        {
            return (null, $"Erro interno: {ex.Message}");
        }
    }
}
