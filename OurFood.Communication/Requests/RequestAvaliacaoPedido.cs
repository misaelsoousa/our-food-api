namespace OurFood.Communication.Requests;

public record RequestAvaliacaoPedido(
    int Avaliacao,
    string? ComentarioAvaliacao = null
);

