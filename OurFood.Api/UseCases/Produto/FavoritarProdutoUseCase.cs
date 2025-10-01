using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using Microsoft.EntityFrameworkCore; 

namespace OurFood.Api.UseCases.Produto;

public interface IToggleFavoritoUseCase
{
    (bool success, string? error, bool isFavorito) Execute(int produtoId, string token);
}

public class ToggleFavoritoUseCase(OurFoodDbContext db, IJwtService jwtService) : IToggleFavoritoUseCase
{
    public (bool success, string? error, bool isFavorito) Execute(int produtoId, string token)
    {
        try
        {
            var userId = jwtService.GetUserIdFromToken(token);

            // Verificar se o produto existe
            var produto = db.Produtos.FirstOrDefault(p => p.Id == produtoId);
            if (produto == null)
            {
                return (false, "Produto não encontrado.", false);
            }

            // Verificar se já está favoritado pelo usuário
            var favoritoExistente = db.ProdutoFavoritos
                .FirstOrDefault(pf => pf.UsuarioId == userId && pf.ProdutoId == produtoId);

            if (favoritoExistente != null)
            {
                // Remove o favorito
                db.ProdutoFavoritos.Remove(favoritoExistente);
                db.SaveChanges();
                return (true, null, false);
            }
            else
            {
                // Adiciona o favorito
                var novoFavorito = new Entities.ProdutoFavorito
                {
                    UsuarioId = (int)userId,
                    ProdutoId = produtoId,
                    CreatedAt = DateTime.Now
                };

                db.ProdutoFavoritos.Add(novoFavorito);
                db.SaveChanges();
                return (true, null, true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao alternar favorito: {ex.Message}");
            return (false, "Erro interno ao salvar no banco de dados.", false);
        }
    }
}