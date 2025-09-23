using ourFood.API.Infrastructure;

namespace ourFood.API.useCases.Categoria;

public class DeleteCategoriaUseCase
{
	public bool Execute(int id)
	{
		var db = new ourFoodDbContext();
		var entity = db.Categorias.FirstOrDefault(x => x.Id == id);
		if (entity == null) return false;
		db.Categorias.Remove(entity);
		db.SaveChanges();
		return true;
	}
}




