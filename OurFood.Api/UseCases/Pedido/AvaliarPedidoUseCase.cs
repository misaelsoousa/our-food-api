using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Requests;

namespace OurFood.Api.UseCases.Pedido;

public interface IAvaliarPedidoUseCase
{
    (bool success, string? error) Execute(int pedidoId, RequestAvaliacaoPedido request, string token);
}

public class AvaliarPedidoUseCase(OurFoodDbContext db, IJwtService jwtService) : IAvaliarPedidoUseCase
{
    public (bool success, string? error) Execute(int pedidoId, RequestAvaliacaoPedido request, string token)
    {
        var userId = jwtService.GetUserIdFromToken(token);

        var pedido = db.Pedidos.FirstOrDefault(p => p.Id == pedidoId && p.UsuarioId == userId);
        if (pedido == null)
        {
            return (false, "Pedido não encontrado ou você não tem permissão para avaliá-lo.");
        }

        // Verificar se o pedido foi entregue
        if (pedido.Status != "entregue")
        {
            return (false, "Apenas pedidos entregues podem ser avaliados.");
        }

        // Validar avaliação (1-5)
        if (request.Avaliacao < 1 || request.Avaliacao > 5)
        {
            return (false, "Avaliação deve estar entre 1 e 5.");
        }

        pedido.Avaliacao = request.Avaliacao;
        pedido.ComentarioAvaliacao = request.ComentarioAvaliacao;
        pedido.UpdatedAt = DateTime.Now;

        db.SaveChanges();
        return (true, null);
    }
}