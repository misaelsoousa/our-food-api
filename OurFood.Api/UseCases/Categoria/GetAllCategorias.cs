using System.Linq;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Categoria;

public interface IGetAllCategorias
{
    ResponseAllCategorias Execute();
}

public class GetAllCategorias(OurFoodDbContext db) : IGetAllCategorias
{
    public ResponseAllCategorias Execute()
    {
        var response = db.Categorias.Select(c => 
            new ResponseCategoria(
            c.Id, c.Nome, c.CorHex, c.Imagem)
        ).ToList();
        
        return new ResponseAllCategorias(Categorias: response);
    }
}
