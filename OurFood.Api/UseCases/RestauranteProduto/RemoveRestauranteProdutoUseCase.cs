using System.Linq;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Requests;

namespace OurFood.Api.UseCases.RestauranteProduto;

public interface IRemoveRestauranteProdutoUseCase
{
    (bool ok, string? error) Execute(RequestRelacaoRestauranteProduto request);
}

public class RemoveRestauranteProdutoUseCase(OurFoodDbContext db)
    : IRemoveRestauranteProdutoUseCase
{
    public (bool ok, string? error) Execute(RequestRelacaoRestauranteProduto request)
    {
        var rel = db.RestaurantesProdutos.FirstOrDefault(x => x.RestauranteId == request.RestauranteId && x.ProdutoId == request.ProdutoId);
        if (rel == null) return (false, "Relação não encontrada");
        db.RestaurantesProdutos.Remove(rel);
        db.SaveChanges();
        return (true, null);
    }
}



