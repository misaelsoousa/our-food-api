using System.Linq;
using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Restaurante;

public interface IGetRestauranteDetalhe
{
    (ResponseRestauranteDetalhe? response, string? error) Execute(int restauranteId);
}

public class GetRestauranteDetalhe(OurFoodDbContext db) : IGetRestauranteDetalhe
{
    public (ResponseRestauranteDetalhe? response, string? error) Execute(int restauranteId)
    {
        var restaurante = db.Restaurantes.FirstOrDefault(r => r.Id == restauranteId);
        if (restaurante == null) return (null, "Restaurante não encontrado");

        var produtos = db.RestaurantesProdutos
            .Where(rp => rp.RestauranteId == restauranteId)
            .Include(rp => rp.Produto)
            .ThenInclude(p => p.Categoria)
            .Include(rp => rp.Produto)
            .ThenInclude(p => p.Restaurante)
            .Select(rp => rp.Produto)
            .ToList();

        var produtosList = produtos.Select(p => new ResponseProduto(
            p.Id,
            p.Nome,
            p.Imagem,
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
            restaurante.Imagem,
            produtosList
        );

        return (resp, null);
    }
}
