namespace OurFood.Communication.Requests;

public record RequestPedido(
    int RestauranteId,
    decimal TaxaEntrega,
    string EnderecoEntrega,
    string MetodoPagamento,
    string? Observacoes,
    List<RequestPedidoItem> Itens
);
