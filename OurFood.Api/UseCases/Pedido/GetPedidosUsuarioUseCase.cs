using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Pedido;

public interface IGetPedidosUsuarioUseCase
{
    List<ResponsePedidoSimples> Execute(string token);
}

public class GetPedidosUsuarioUseCase(OurFoodDbContext db, IJwtService jwtService) : IGetPedidosUsuarioUseCase
{
    public List<ResponsePedidoSimples> Execute(string token)
    {
        var userId = jwtService.GetUserIdFromToken(token);

        var pedidos = db.Pedidos
            .Include(p => p.Restaurante)
            .Where(p => p.UsuarioId == userId)
            .OrderByDescending(p => p.DataPedido)
            .ToList();

        return pedidos.Select(p => new ResponsePedidoSimples(
            p.Id,
            p.Restaurante.Nome,
            p.Status,
            p.DataPedido,
            p.ValorFinal,
            p.Avaliacao
        )).ToList();
    }
}
