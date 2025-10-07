using System.ComponentModel.DataAnnotations;

namespace OurFood.Communication.Requests;

public record RequestUpdateProduto(
    [Required(ErrorMessage = "Nome é obrigatório")]
    string Nome,
    
    [Required(ErrorMessage = "CategoriaId é obrigatório")]
    int CategoriaId,
    
    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    decimal Preco,
    
    string? Descricao,
    
    [Required(ErrorMessage = "RestauranteId é obrigatório")]
    int RestauranteId
);

