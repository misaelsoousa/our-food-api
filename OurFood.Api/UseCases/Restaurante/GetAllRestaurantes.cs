using System.Linq;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Restaurante;

public interface IGetAllRestaurantes
{
    ResponseAllRestaurantes Execute();
}

public class GetAllRestaurantes(OurFoodDbContext db) : IGetAllRestaurantes
{
    public ResponseAllRestaurantes Execute()
    {
        var list = db.Restaurantes.Select(r => new ResponseRestaurante(
            r.Id,
            r.Nome,
            r.Imagem
        )).ToList();
        return new ResponseAllRestaurantes(Restaurantes:list);
    }
}



