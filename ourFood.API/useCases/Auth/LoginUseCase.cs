using ourFood.API.Infrastructure;
using ourFood.Communication.Requests;
using ourFood.Communication.Responses;

namespace ourFood.API.useCases.Auth;

public class LoginUseCase
{
	public (ResponseAuth? response, string? error) Execute(RequestLogin request, Func<int, string> generateToken)
	{
		var db = new ourFoodDbContext();
		var user = db.Usuarios.FirstOrDefault(u => u.Email == request.Email);
		if (user == null) return (null, "Usuário ou senha inválidos");
		if (!BCrypt.Net.BCrypt.Verify(request.Senha, user.SenhaHash)) return (null, "Usuário ou senha inválidos");
		var token = generateToken(user.Id);
		return (new ResponseAuth { Token = token }, null);
	}
}




