using System.ComponentModel.DataAnnotations;

namespace OurFood.Communication.Requests;

public record RequestUpdateRestaurante(
    [Required(ErrorMessage = "Nome é obrigatório")]
    string Nome
);
