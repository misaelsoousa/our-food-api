using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Pedido;

public interface IUpdatePedidoUseCase
{
    (ResponsePedido? response, string? error) Execute(int id, RequestUpdatePedido request);
}

public class UpdatePedidoUseCase(OurFoodDbContext db) : IUpdatePedidoUseCase
{
    public (ResponsePedido? response, string? error) Execute(int id, RequestUpdatePedido request)
    {
        try
        {
            // Buscar o pedido existente
            var pedido = db.Pedidos
                .Include(p => p.Restaurante)
                .Include(p => p.PedidoItens)
                .ThenInclude(pi => pi.Produto)
                .FirstOrDefault(p => p.Id == id);
            
            if (pedido == null)
                return (null, "Pedido não encontrado");

            // Atualizar os dados do pedido
            pedido.Status = request.Status;
            pedido.Observacoes = request.Observacoes;
            pedido.MetodoPagamento = request.MetodoPagamento;
            pedido.EnderecoEntrega = request.EnderecoEntrega;
            
            if (request.TaxaEntrega.HasValue)
                pedido.TaxaEntrega = request.TaxaEntrega.Value;
            
            if (request.ValorFinal.HasValue)
                pedido.ValorFinal = request.ValorFinal.Value;

            // Se o status foi alterado para "Entregue", definir a data de entrega
            if (request.Status == "Entregue" && pedido.DataEntrega == null)
            {
                pedido.DataEntrega = DateTime.UtcNow;
            }

            // Salvar as alterações
            db.SaveChanges();

            // Mapear os itens do pedido
            var pedidoItens = pedido.PedidoItens.Select(pi => new ResponsePedidoItem(
                pi.Id,
                pi.ProdutoId,
                pi.Produto.Nome,
                pi.Quantidade,
                pi.PrecoUnitario,
                pi.PrecoTotal,
                pi.ObservacoesItem
            )).ToList();

            // Retornar o pedido atualizado
            return (new ResponsePedido(
                pedido.Id,
                pedido.UsuarioId,
                "Usuário", // Nome do usuário não está disponível na entidade
                pedido.RestauranteId,
                pedido.Restaurante.Nome,
                pedido.Status,
                pedido.DataPedido,
                pedido.DataEntrega,
                pedido.ValorTotal,
                pedido.TaxaEntrega,
                pedido.ValorFinal,
                pedido.EnderecoEntrega ?? string.Empty,
                pedido.Observacoes,
                pedido.MetodoPagamento,
                pedido.Avaliacao,
                pedido.ComentarioAvaliacao,
                pedidoItens
            ), null);
        }
        catch (Exception ex)
        {
            return (null, $"Erro interno: {ex.Message}");
        }
    }
}
