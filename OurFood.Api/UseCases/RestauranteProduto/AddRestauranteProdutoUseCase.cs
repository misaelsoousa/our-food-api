using System.Linq;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Requests;

namespace OurFood.Api.UseCases.RestauranteProduto;

public interface IAddRestauranteProdutoUseCase
{
    (bool ok, string? error) Execute(RequestRelacaoRestauranteProduto request);
}

public class AddRestauranteProdutoUseCase(OurFoodDbContext db)
    : IAddRestauranteProdutoUseCase
{
    public (bool ok, string? error) Execute(RequestRelacaoRestauranteProduto request)
    {
        var restaurante = db.Restaurantes.FirstOrDefault(r => r.Id == request.RestauranteId);
        if (restaurante == null) return (false, "Restaurante não encontrado");
        var produto = db.Produtos.FirstOrDefault(p => p.Id == request.ProdutoId);
        if (produto == null) return (false, "Produto não encontrado");

        var exists = db.RestaurantesProdutos.FirstOrDefault(x => x.RestauranteId == request.RestauranteId && x.ProdutoId == request.ProdutoId);
        if (exists != null) return (true, null);

        db.RestaurantesProdutos.Add(new Entities.RestauranteProduto
        {
            RestauranteId = request.RestauranteId,
            ProdutoId = request.ProdutoId
        });
        db.SaveChanges();
        return (true, null);
    }
}
