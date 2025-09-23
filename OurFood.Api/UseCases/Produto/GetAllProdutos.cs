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
        var list = db.Produtos.Include(p => p.Categoria).Select(p => new ResponseProduto
        (
            p.Id,
            p.Nome,
            p.Imagem,
            p.Preco,
            p.CategoriaId ?? 0,
            p.Categoria.Nome
        )).ToList();
        
        return new ResponseAllProdutos(Produtos: list);
    }
}
