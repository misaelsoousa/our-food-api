using System.Text;
using System.Text.Json;

namespace OurFood.Api.Services;

public static class JwtUtility
{
    public static string GetClaimValue(string token, string claimType)
    {
        // 1. Divide o token nas três partes (Header.Payload.Signature)
        var parts = token.Split('.');
            
        // Verifica se o formato é válido
        if (parts.Length != 3) return null;

        // A parte do meio é o Payload
        var payloadBase64 = parts[1];
            
        // 2. Ajusta o formato Base64Url para o Base64 padrão (padding)
        string base64 = payloadBase64.Replace('-', '+').Replace('_', '/');
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        try
        {
            // 3. Converte a string Base64 decodificada em bytes
            var payloadBytes = Convert.FromBase64String(base64);
                
            // 4. Converte os bytes em uma string JSON (Payload)
            var payloadJson = Encoding.UTF8.GetString(payloadBytes);
                
            // 5. Analisa o JSON e extrai o valor do ClaimType (e.g., "nameid" ou "sub")
            using (JsonDocument doc = JsonDocument.Parse(payloadJson))
            {
                if (doc.RootElement.TryGetProperty(claimType, out JsonElement element))
                {
                    // Retorna o valor da Claim como string
                    return element.ToString();
                }
            }
        }
        catch 
        {
            // Qualquer erro na decodificação ou parsing JSON resulta em null
            return null;
        }
            
        return null;
    }
}