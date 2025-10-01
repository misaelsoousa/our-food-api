using System.Linq;
using OurFood.Api.Infrastructure;

namespace OurFood.Api.UseCases.Produto;

public interface IDeleteProdutoUseCase
{
    bool Execute(int id);
}

public class DeleteProdutoUseCase(OurFoodDbContext db) : IDeleteProdutoUseCase
{
    public bool Execute(int id)
    {
        var entity = db.Produtos.FirstOrDefault(x => x.Id == id);
        if (entity == null) return false;
        db.Produtos.Remove(entity);
        db.SaveChanges();
        return true;
    }
}




