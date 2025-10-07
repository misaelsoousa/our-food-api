using System.ComponentModel.DataAnnotations;

namespace OurFood.Communication.Requests;

public record RequestUpdatePedido(
    [Required(ErrorMessage = "Status é obrigatório")]
    string Status,
    
    string? Observacoes,
    
    [Required(ErrorMessage = "Método de pagamento é obrigatório")]
    string MetodoPagamento,
    
    string? EnderecoEntrega,
    
    decimal? TaxaEntrega,
    
    decimal? ValorFinal
);

