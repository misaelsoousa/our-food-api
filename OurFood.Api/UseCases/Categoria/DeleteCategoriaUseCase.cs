using System.Linq;
using OurFood.Api.Infrastructure;

namespace OurFood.Api.UseCases.Categoria;

public interface IDeleteCategoriaUseCase
{
    bool Execute(int id);
}

public class DeleteCategoriaUseCase(OurFoodDbContext db) : IDeleteCategoriaUseCase
{
    public bool Execute(int id)
    {
        var entity = db.Categorias.FirstOrDefault(x => x.Id == id);
        if (entity == null) return false;
        db.Categorias.Remove(entity);
        db.SaveChanges();
        return true;
    }
}







