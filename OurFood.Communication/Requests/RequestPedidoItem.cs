namespace OurFood.Communication.Requests;

public record RequestPedidoItem(
    int ProdutoId,
    int Quantidade,
    string? ObservacoesItem = null
);
