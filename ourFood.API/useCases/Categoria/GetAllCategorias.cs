using ourFood.API.Infrastructure;
using ourFood.Communication.Requests;
using ourFood.Communication.Responses;

namespace ourFood.API.useCases.Categoria;

public class GetAllCategorias
{
    public ResponseAllCategorias Execute()
    {
        var dbContext = new ourFoodDbContext();
        var response = dbContext.Categorias.ToList();
        return new ResponseAllCategorias
        {
            Categorias = response.Select(c => new ResponseCategoria
            {
                Id = c.Id,
                Nome = c.Nome,
                Cor_hex = c.Cor_hex,
                Imagem = c.Imagem
            }).ToList()
        };
    }
}
