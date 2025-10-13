using System.Linq;
using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Restaurante;

public interface IGetRestauranteDetalhe
{
    (ResponseRestauranteDetalhe? response, string? error) Execute(int restauranteId);
}

public class GetRestauranteDetalhe(OurFoodDbContext db, IS3Service s3Service) : IGetRestauranteDetalhe
{
    public (ResponseRestauranteDetalhe? response, string? error) Execute(int restauranteId)
    {
        var restaurante = db.Restaurantes.FirstOrDefault(r => r.Id == restauranteId);
        if (restaurante == null) return (null, "Restaurante não encontrado");

        var produtos = db.Produtos
            .Where(p => p.RestauranteId == restauranteId)
            .Include(p => p.Categoria)
            .Include(p => p.Restaurante)
            .ToList();

        var produtosList = produtos.Select(p => new ResponseProduto(
            p.Id,
            p.Nome,
            !string.IsNullOrEmpty(p.Imagem) ? s3Service.GetFileUrl(p.Imagem) : p.Imagem,
            p.Preco,
            p.CategoriaId ?? 0,
            p.Categoria.Nome ?? string.Empty,
            p.Descricao ?? string.Empty,
            p.RestauranteId,
            p.Restaurante.Nome
        )).ToList();

        var resp = new ResponseRestauranteDetalhe(
            restaurante.Id,
            restaurante.Nome,
            !string.IsNullOrEmpty(restaurante.Imagem) ? s3Service.GetFileUrl(restaurante.Imagem) : restaurante.Imagem,
            produtosList
        );

        return (resp, null);
    }
}
