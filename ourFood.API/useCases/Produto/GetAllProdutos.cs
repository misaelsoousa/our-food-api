using Microsoft.EntityFrameworkCore;
using ourFood.API.Infrastructure;
using ourFood.Communication.Responses;

namespace ourFood.API.useCases.Produto;

public class GetAllProdutos
{
	public ResponseAllProdutos Execute()
	{
		var db = new ourFoodDbContext();
		var list = db.Produtos.Include(p => p.Categoria).ToList();
		return new ResponseAllProdutos
		{
			Produtos = list.Select(p => new ResponseProduto
			{
				Id = p.Id,
				Nome = p.Nome,
				Imagem = p.Imagem,
				Preco = p.Preco,
				CategoriaId = p.CategoriaId ?? 0,
				CategoriaNome = p.Categoria?.Nome
			}).ToList()
		};
	}
}


