namespace OurFood.Communication.Responses;

public record ResponsePedido(
    int Id,
    int UsuarioId,
    string UsuarioNome,
    int RestauranteId,
    string RestauranteNome,
    string Status,
    DateTime DataPedido,
    DateTime? DataEntrega,
    decimal ValorTotal,
    decimal TaxaEntrega,
    decimal ValorFinal,
    string EnderecoEntrega,
    string? Observacoes,
    string MetodoPagamento,
    int? Avaliacao,
    string? ComentarioAvaliacao,
    List<ResponsePedidoItem> Itens
);

