using ourFood.API.Infrastructure;
using ourFood.Communication.Responses;

namespace ourFood.API.useCases.Restaurante;

public class GetAllRestaurantes
{
	public ResponseAllRestaurantes Execute()
	{
		var db = new ourFoodDbContext();
		var list = db.Restaurantes.ToList();
		return new ResponseAllRestaurantes
		{
			Restaurantes = list.Select(r => new ResponseRestaurante
			{
				Id = r.Id,
				Nome = r.Nome,
				Imagem = r.Imagem
			}).ToList()
		};
	}
}




