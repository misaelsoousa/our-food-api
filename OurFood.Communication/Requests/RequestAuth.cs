namespace OurFood.Communication.Requests;

public record RequestAuth (
    string Email,
    string Nome,
    string Senha
);

