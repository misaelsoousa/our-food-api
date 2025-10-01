using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Pedido;

public interface ICriarPedidoUseCase
{
    ResponsePedido Execute(RequestPedido request, string token);
}

public class CriarPedidoUseCase(OurFoodDbContext db, IJwtService jwtService) : ICriarPedidoUseCase
{
    public ResponsePedido Execute(RequestPedido request, string token)
    {
        var userId = jwtService.GetUserIdFromToken(token);

        // Verificar se o restaurante existe
        var restaurante = db.Restaurantes.FirstOrDefault(r => r.Id == request.RestauranteId);
        if (restaurante == null)
        {
            throw new Exception("Restaurante não encontrado.");
        }

        // Verificar se o usuário existe
        var usuario = db.Usuarios.FirstOrDefault(u => u.Id == userId);
        if (usuario == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        // Buscar todos os produtos necessários de uma vez
        var produtoIds = request.Itens.Select(i => i.ProdutoId).Distinct().ToList();
        var produtos = db.Produtos.Where(p => produtoIds.Contains(p.Id)).ToDictionary(p => p.Id);
        
        // Verificar se todos os produtos existem
        foreach (var produtoId in produtoIds)
        {
            if (!produtos.ContainsKey(produtoId))
            {
                throw new Exception($"Produto com ID {produtoId} não encontrado.");
            }
        }

        // Calcular valor total dos itens
        decimal valorTotal = request.Itens.Sum(item => produtos[item.ProdutoId].Preco * item.Quantidade);
        
        // Calcular valor final (valor total + taxa de entrega)
        decimal valorFinal = valorTotal + request.TaxaEntrega;

        // Criar o pedido
        var pedido = new Entities.Pedido
        {
            UsuarioId = (int)userId,
            RestauranteId = request.RestauranteId,
            Status = "pendente",
            DataPedido = DateTime.Now,
            ValorTotal = valorTotal,
            TaxaEntrega = request.TaxaEntrega,
            ValorFinal = valorFinal,
            EnderecoEntrega = request.EnderecoEntrega,
            Observacoes = request.Observacoes,
            MetodoPagamento = request.MetodoPagamento,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        db.Pedidos.Add(pedido);
        db.SaveChanges();

        // Criar os itens do pedido
        foreach (var itemRequest in request.Itens)
        {
            var produto = produtos[itemRequest.ProdutoId];

            var pedidoItem = new Entities.PedidoItem
            {
                PedidoId = pedido.Id,
                ProdutoId = itemRequest.ProdutoId,
                Quantidade = itemRequest.Quantidade,
                PrecoUnitario = produto.Preco,
                PrecoTotal = produto.Preco * itemRequest.Quantidade,
                ObservacoesItem = itemRequest.ObservacoesItem
            };

            db.PedidoItens.Add(pedidoItem);
        }

        db.SaveChanges();

        // Buscar o pedido completo com relacionamentos
        var pedidoCompleto = db.Pedidos
            .Include(p => p.Usuario)
            .Include(p => p.Restaurante)
            .Include(p => p.PedidoItens)
                .ThenInclude(pi => pi.Produto)
            .FirstOrDefault(p => p.Id == pedido.Id);

        if (pedidoCompleto == null)
        {
            throw new Exception("Erro ao buscar pedido criado.");
        }

        return new ResponsePedido(
            pedidoCompleto.Id,
            pedidoCompleto.UsuarioId,
            pedidoCompleto.Usuario.Nome,
            pedidoCompleto.RestauranteId,
            pedidoCompleto.Restaurante.Nome,
            pedidoCompleto.Status,
            pedidoCompleto.DataPedido,
            pedidoCompleto.DataEntrega,
            pedidoCompleto.ValorTotal,
            pedidoCompleto.TaxaEntrega,
            pedidoCompleto.ValorFinal,
            pedidoCompleto.EnderecoEntrega,
            pedidoCompleto.Observacoes,
            pedidoCompleto.MetodoPagamento,
            pedidoCompleto.Avaliacao,
            pedidoCompleto.ComentarioAvaliacao,
            pedidoCompleto.PedidoItens.Select(pi => new ResponsePedidoItem(
                pi.Id,
                pi.ProdutoId,
                pi.Produto.Nome,
                pi.Quantidade,
                pi.PrecoUnitario,
                pi.PrecoTotal,
                pi.ObservacoesItem
            )).ToList()
        );
    }
}