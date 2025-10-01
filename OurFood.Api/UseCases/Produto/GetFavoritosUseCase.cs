using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Responses;
using Microsoft.EntityFrameworkCore;

namespace OurFood.Api.UseCases.Produto;

public interface IGetFavoritosUseCase
{
    ResponseAllProdutos Execute(string token);
}

public class GetFavoritosUseCase(OurFoodDbContext db, IJwtService jwtService) : IGetFavoritosUseCase
{
    public ResponseAllProdutos Execute(string token)
    {
        var userId = jwtService.GetUserIdFromToken(token);

        var produtosFavoritos = db.ProdutoFavoritos
            .Where(pf => pf.UsuarioId == userId)
            .Include(pf => pf.Produto)
            .ThenInclude(p => p.Categoria)
            .Select(pf => pf.Produto)
            .ToList();

        var produtosList = produtosFavoritos.Select(p => new ResponseProduto(
            p.Id,
            p.Nome,
            p.Imagem,
            p.Preco,
            p.CategoriaId ?? 0,
            p.Categoria.Nome ?? string.Empty,
            p.Descricao ?? string.Empty
        )).ToList();

        return new ResponseAllProdutos(Produtos: produtosList);
    }
}
