using BCrypt.Net;
using ourFood.API.Entitys;
using ourFood.API.Infrastructure;
using ourFood.Communication.Requests;
using ourFood.Communication.Responses;

namespace ourFood.API.useCases.Auth;

public class RegisterUsuarioUseCase
{
	public ResponseAuth Execute(RequestAuth request, string token)
	{
		var db = new ourFoodDbContext();
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
		return new ResponseAuth { Token = token };
	}
}




