using System;
using System.Linq;
using BCrypt.Net;
using OurFood.Api.Entities;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Auth;

public interface IRegisterUsuarioUseCase
{
    ResponseAuth Execute(RequestAuth request, string token);
}

public class RegisterUsuarioUseCase(OurFoodDbContext db) : IRegisterUsuarioUseCase
{
    public ResponseAuth Execute(RequestAuth request, string token)
    {
        if (db.Usuarios.Any(u => u.Email == request.Email))
        {
            throw new Exception("Email já registrado");
        }
        var usuario = new Usuario
        {
            Email = request.Email,
            Nome = request.Nome,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha)
        };
        db.Usuarios.Add(usuario);
        db.SaveChanges();
        return new ResponseAuth(Token: token);
    }
}



