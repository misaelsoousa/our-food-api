using ourFood.API.Infrastructure;

namespace ourFood.API.useCases.Restaurante;

public class DeleteRestauranteUseCase
{
	public bool Execute(int id)
	{
		var db = new ourFoodDbContext();
		var entity = db.Restaurantes.FirstOrDefault(x => x.Id == id);
		if (entity == null) return false;
		db.Restaurantes.Remove(entity);
		db.SaveChanges();
		return true;
	}
}




