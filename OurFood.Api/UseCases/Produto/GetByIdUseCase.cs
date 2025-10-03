using System.Linq;
using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Produto;
public interface IGetByIdUseCase
{
    ResponseProduto Execute(int id);
}
public class GetByIdUseCase(OurFoodDbContext db, IS3Service s3Service) : IGetByIdUseCase
{
    public ResponseProduto Execute(int id)
    {
        try
        {
            var produto = db.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Restaurante)
                .Where(p => p.Id == id)
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
                ))
                .FirstOrDefault(); // Retorna o objeto ResponseProduto ou null

            // Retorna o ResponseProduto encontrado (pode ser null)
            return produto;
        }
        catch (Exception ex)
        {
            // Log do erro para debug
            Console.WriteLine($"Erro ao buscar produto por ID: {ex.Message}");
            return null;
        }
    }
}