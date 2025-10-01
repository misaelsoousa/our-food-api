namespace OurFood.Communication.Responses;

public record ResponsePedidoItem(
    int Id,
    int ProdutoId,
    string ProdutoNome,
    int Quantidade,
    decimal PrecoUnitario,
    decimal PrecoTotal,
    string? ObservacoesItem = null
);

