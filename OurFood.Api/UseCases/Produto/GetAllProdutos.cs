using System.Linq;
using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Produto;

public interface IGetAllProdutos
{
    ResponseAllProdutos Execute();
}

public class GetAllProdutos(OurFoodDbContext db) : IGetAllProdutos
{
    public ResponseAllProdutos Execute()
    {
        var list = db.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.Restaurante)
            .Select(p => new ResponseProduto
            (
                p.Id,
                p.Nome,
                p.Imagem,
                p.Preco,
                p.CategoriaId ?? 0,
                p.Categoria.Nome ?? string.Empty,
                p.Descricao ?? string.Empty,
                p.RestauranteId,
                p.Restaurante.Nome
            )).ToList();
        
        return new ResponseAllProdutos(Produtos: list);
    }
}
