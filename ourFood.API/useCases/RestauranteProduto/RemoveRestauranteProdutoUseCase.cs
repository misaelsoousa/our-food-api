using ourFood.API.Infrastructure;
using ourFood.Communication.Requests;

namespace ourFood.API.useCases.RestauranteProduto;

public class RemoveRestauranteProdutoUseCase
{
	public (bool ok, string? error) Execute(RequestRelacaoRestauranteProduto request)
	{
		var db = new ourFoodDbContext();
		var rel = db.RestaurantesProdutos.FirstOrDefault(x => x.RestauranteId == request.RestauranteId && x.ProdutoId == request.ProdutoId);
		if (rel == null) return (false, "Relação não encontrada");
		db.RestaurantesProdutos.Remove(rel);
		db.SaveChanges();
		return (true, null);
	}
}




