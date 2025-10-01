using OurFood.Api.Infrastructure;
using OurFood.Communication.Requests;

namespace OurFood.Api.UseCases.Pedido;

public interface IUpdatePedidoStatusUseCase
{
    (bool success, string? error) Execute(int pedidoId, RequestUpdatePedidoStatus request);
}

public class UpdatePedidoStatusUseCase(OurFoodDbContext db) : IUpdatePedidoStatusUseCase
{
    public (bool success, string? error) Execute(int pedidoId, RequestUpdatePedidoStatus request)
    {
        var pedido = db.Pedidos.FirstOrDefault(p => p.Id == pedidoId);
        if (pedido == null)
        {
            return (false, "Pedido não encontrado.");
        }

        // Validar status
        var statusValidos = new[] { "pendente", "confirmado", "preparando", "entregue", "cancelado" };
        if (!statusValidos.Contains(request.Status))
        {
            return (false, "Status inválido.");
        }

        pedido.Status = request.Status;
        pedido.UpdatedAt = DateTime.Now;

        // Se o status for "entregue", definir data de entrega
        if (request.Status == "entregue")
        {
            pedido.DataEntrega = DateTime.Now;
        }

        db.SaveChanges();
        return (true, null);
    }
}