using System;
using System.Linq;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Auth;

public interface ILoginUseCase
{
    (ResponseAuth? response, string? error) Execute(RequestLogin request, Func<int, string> generateToken);
}

public class LoginUseCase(OurFoodDbContext db) : ILoginUseCase
{
    public (ResponseAuth? response, string? error) Execute(RequestLogin request, Func<int, string> generateToken)
    {
        var user = db.Usuarios.FirstOrDefault(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Senha, user.SenhaHash)) return (null, "Usuário ou senha inválidos");
        var token = generateToken(user.Id);
        return (new ResponseAuth(Token: token), null);
    }
}
