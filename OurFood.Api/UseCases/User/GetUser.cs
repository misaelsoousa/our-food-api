using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.User;

public interface IGetUser
{
    ResponseUser Execute(string token);
}

public class GetUser(OurFoodDbContext db, IJwtService jwtService) : IGetUser
{
    public ResponseUser Execute(string token)
    {
        var userId = jwtService.GetUserIdFromToken(token);

        var user = db.Usuarios.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            throw new Exception("Usuário não encontrado.");
        }

        return new ResponseUser(
            user.Id,
            user.Nome,
            user.Email
        );
    }
}