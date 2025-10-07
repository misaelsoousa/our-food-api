using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Produto;

public interface IRegisterProdutoUseCase
{
    Task<(ResponseProduto? response, string? error)> Execute(RequestProduto request, IFormFile? imagemFile);
}

public class RegisterProdutoUseCase(OurFoodDbContext db, IS3Service s3Service) : IRegisterProdutoUseCase
{
    public async Task<(ResponseProduto? response, string? error)> Execute(RequestProduto request, IFormFile? imagemFile)
    {
        var categoria = db.Categorias.FirstOrDefault(c => c.Id == request.CategoriaId);
        if (categoria == null) return (null, "Categoria não encontrada");

        var restaurante = db.Restaurantes.FirstOrDefault(r => r.Id == request.RestauranteId);
        if (restaurante == null) return (null, "Restaurante não encontrado");

        string? caminhoImagem = null;
        if (imagemFile != null && imagemFile.Length > 0)
        {
            caminhoImagem = await s3Service.UploadFileAsync(imagemFile, "produtos");
        }

        var entity = new Entities.Produto
        {
            Nome = request.Nome,
            Imagem = caminhoImagem,
            Preco = request.Preco,
            Descricao = request.Descricao,
            CategoriaId = request.CategoriaId,
            RestauranteId = request.RestauranteId
        };

        db.Produtos.Add(entity);
        db.SaveChanges();

        return (new ResponseProduto
        (
            entity.Id,
            entity.Nome,
            entity.Imagem,
            entity.Preco,
            categoria.Id,
            categoria.Nome ?? string.Empty,
            entity.Descricao ?? string.Empty,
            restaurante.Id,
            restaurante.Nome
        ), null);
    }

}
