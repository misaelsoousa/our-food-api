namespace OurFood.Api.Services;

public interface IJwtService
{
    long GetUserIdFromToken(string token);
}

public class JwtService : IJwtService
{
    public long GetUserIdFromToken(string token)
    {
        // O ClaimTypes.NameIdentifier é serializado como o nome completo da claim
        var nameIdentifierClaim = JwtUtility.GetClaimValue(token, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        var subClaim = JwtUtility.GetClaimValue(token, "sub");
        var nameidClaim = JwtUtility.GetClaimValue(token, "nameid");
        
        // Tenta com o nome completo da claim primeiro
        string userIdString = nameIdentifierClaim ?? subClaim ?? nameidClaim;
        
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new Exception("Token inválido: Nenhuma claim de ID de usuário encontrada.");
        }
        
        if (long.TryParse(userIdString, out long userId))
        {
            return userId;
        }
        
        throw new Exception($"Token inválido: ID do usuário '{userIdString}' não é um número válido.");
    }
}