using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Pedido;

public interface IGetPedidoDetalheUseCase
{
    (ResponsePedido? response, string? error) Execute(int pedidoId, string token);
}

public class GetPedidoDetalheUseCase(OurFoodDbContext db, IJwtService jwtService) : IGetPedidoDetalheUseCase
{
    public (ResponsePedido? response, string? error) Execute(int pedidoId, string token)
    {
        var userId = jwtService.GetUserIdFromToken(token);

        var pedido = db.Pedidos
            .Include(p => p.Usuario)
            .Include(p => p.Restaurante)
            .Include(p => p.PedidoItens)
                .ThenInclude(pi => pi.Produto)
            .FirstOrDefault(p => p.Id == pedidoId && p.UsuarioId == userId);

        if (pedido == null)
        {
            return (null, "Pedido não encontrado ou você não tem permissão para visualizá-lo.");
        }

        var response = new ResponsePedido(
            pedido.Id,
            pedido.UsuarioId,
            pedido.Usuario.Nome,
            pedido.RestauranteId,
            pedido.Restaurante.Nome,
            pedido.Status,
            pedido.DataPedido,
            pedido.DataEntrega,
            pedido.ValorTotal,
            pedido.TaxaEntrega,
            pedido.ValorFinal,
            pedido.EnderecoEntrega,
            pedido.Observacoes,
            pedido.MetodoPagamento,
            pedido.Avaliacao,
            pedido.ComentarioAvaliacao,
            pedido.PedidoItens.Select(pi => new ResponsePedidoItem(
                pi.Id,
                pi.ProdutoId,
                pi.Produto.Nome,
                pi.Quantidade,
                pi.PrecoUnitario,
                pi.PrecoTotal,
                pi.ObservacoesItem
            )).ToList()
        );

        return (response, null);
    }
}
