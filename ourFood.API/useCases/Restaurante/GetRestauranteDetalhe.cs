using Microsoft.EntityFrameworkCore;
using ourFood.API.Infrastructure;
using ourFood.Communication.Responses;

namespace ourFood.API.useCases.Restaurante;

public class GetRestauranteDetalhe
{
	public (ResponseRestauranteDetalhe? response, string? error) Execute(int restauranteId)
	{
		var db = new ourFoodDbContext();
		var restaurante = db.Restaurantes.FirstOrDefault(r => r.Id == restauranteId);
		if (restaurante == null) return (null, "Restaurante não encontrado");

		var produtos = db.RestaurantesProdutos
			.Where(rp => rp.RestauranteId == restauranteId)
			.Include(rp => rp.Produto)
			.ThenInclude(p => p.Categoria)
			.Select(rp => rp.Produto)
			.ToList();

		var resp = new ResponseRestauranteDetalhe
		{
			Id = restaurante.Id,
			Nome = restaurante.Nome,
			Imagem = restaurante.Imagem,
			Produtos = produtos.Select(p => new ResponseProduto
			{
				Id = p.Id,
				Nome = p.Nome,
				Imagem = p.Imagem,
				Preco = p.Preco,
				CategoriaId = p.CategoriaId ?? 0,
				CategoriaNome = p.Categoria?.Nome
			}).ToList()
		};

		return (resp, null);
	}
}


