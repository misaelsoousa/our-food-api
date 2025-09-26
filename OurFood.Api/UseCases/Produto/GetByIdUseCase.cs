using System.Linq;
using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Produto;
public interface IGetByIdUseCase
{
    ResponseProduto Execute(int id);
}
public class GetByIdUseCase(OurFoodDbContext db) : IGetByIdUseCase
{
    public ResponseProduto Execute(int id)
    {
        var produto = db.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Id == id)
            .Select(p => new ResponseProduto
            (
                p.Id,
                p.Nome,
                p.Imagem,
                p.Preco,
                p.CategoriaId ?? 0,
                p.Categoria.Nome,
                p.Descricao,
                p.Favorito
            ))
            .FirstOrDefault(); // Retorna o objeto ResponseProduto ou null

        // Retorna o ResponseProduto encontrado (pode ser null)
        return produto;
    }
}