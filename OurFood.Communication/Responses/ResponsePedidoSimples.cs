namespace OurFood.Communication.Responses;

public record ResponsePedidoSimples(
    int Id,
    string RestauranteNome,
    string Status,
    DateTime DataPedido,
    decimal ValorFinal,
    int? Avaliacao
);

