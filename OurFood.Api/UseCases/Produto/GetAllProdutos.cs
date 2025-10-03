using System.Linq;
using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Produto;

public interface IGetAllProdutos
{
    ResponseAllProdutos Execute();
}

public class GetAllProdutos(OurFoodDbContext db, IS3Service s3Service) : IGetAllProdutos
{
    public ResponseAllProdutos Execute()
    {
        try
        {
            var list = db.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Restaurante)
                .Select(p => new ResponseProduto
                (
                    p.Id,
                    p.Nome,
                    !string.IsNullOrEmpty(p.Imagem) ? s3Service.GetFileUrl(p.Imagem) : p.Imagem,
                    p.Preco,
                    p.CategoriaId ?? 0,
                    p.Categoria.Nome ?? string.Empty,
                    p.Descricao ?? string.Empty,
                    p.RestauranteId,
                    p.Restaurante.Nome
                )).ToList();
            
            return new ResponseAllProdutos(Produtos: list);
        }
        catch (Exception ex)
        {
            // Log do erro para debug
            Console.WriteLine($"Erro ao buscar produtos: {ex.Message}");
            return new ResponseAllProdutos(Produtos: new List<ResponseProduto>());
        }
    }
}
