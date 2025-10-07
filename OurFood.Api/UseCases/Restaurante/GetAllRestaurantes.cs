using System.Linq;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Restaurante;

public interface IGetAllRestaurantes
{
    ResponseAllRestaurantes Execute();
}

public class GetAllRestaurantes(OurFoodDbContext db, IS3Service s3Service) : IGetAllRestaurantes
{
    public ResponseAllRestaurantes Execute()
    {
        var list = db.Restaurantes.Select(r => new ResponseRestaurante(
            r.Id,
            r.Nome,
            !string.IsNullOrEmpty(r.Imagem) ? s3Service.GetFileUrl(r.Imagem) : r.Imagem
        )).ToList();
        return new ResponseAllRestaurantes(Restaurantes:list);
    }
}








