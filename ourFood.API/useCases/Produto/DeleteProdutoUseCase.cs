using ourFood.API.Infrastructure;

namespace ourFood.API.useCases.Produto;

public class DeleteProdutoUseCase
{
	public bool Execute(int id)
	{
		var db = new ourFoodDbContext();
		var entity = db.Produtos.FirstOrDefault(x => x.Id == id);
		if (entity == null) return false;
		db.Produtos.Remove(entity);
		db.SaveChanges();
		return true;
	}
}




