using System.Linq;
using OurFood.Api.Infrastructure;
using Microsoft.EntityFrameworkCore; 

namespace OurFood.Api.UseCases.Produto;

public interface IToggleFavoritoUseCase
{
    (bool success, string? error) Execute(int produtoId);
}

public class ToggleFavoritoUseCase(OurFoodDbContext db) : IToggleFavoritoUseCase
{
    public (bool success, string? error) Execute(int produtoId)
    {
        var produto = db.Produtos.SingleOrDefault(p => p.Id == produtoId);

        if (produto == null)
        {
            return (false, "Produto não encontrado.");
        }

        produto.Favorito = !produto.Favorito;

        try
        {
            db.SaveChanges();
            
            return (true, null); 
        }
        catch (DbUpdateConcurrencyException)
        {
            return (false, "Erro de concorrência ao atualizar o produto.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao alternar favorito: {ex.Message}");
            return (false, "Erro interno ao salvar no banco de dados.");
        }
    }
}