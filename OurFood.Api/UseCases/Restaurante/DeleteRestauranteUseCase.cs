using System.Linq;
using OurFood.Api.Infrastructure;

namespace OurFood.Api.UseCases.Restaurante;

public interface IDeleteRestauranteUseCase
{
    bool Execute(int id);
}

public class DeleteRestauranteUseCase(OurFoodDbContext db) : IDeleteRestauranteUseCase
{
    public bool Execute(int id)
    {
        var entity = db.Restaurantes.FirstOrDefault(x => x.Id == id);
        if (entity == null) return false;
        db.Restaurantes.Remove(entity);
        db.SaveChanges();
        return true;
    }
}



