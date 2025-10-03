using System.ComponentModel.DataAnnotations;

namespace OurFood.Communication.Requests;

public record RequestUpdateCategoria(
    [Required(ErrorMessage = "Nome é obrigatório")]
    string Nome,
    
    [Required(ErrorMessage = "CorHex é obrigatório")]
    string CorHex
);
