using Microsoft.AspNetCore.Mvc;
using OurFood.Api.UseCases.User;

namespace OurFood.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController(IGetUser getUser) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetUser()
    {
        try
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault() ?? string.Empty;

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Token de autorização não fornecido ou formato inválido");
            }

            string jwt = authorizationHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrEmpty(jwt))
            {
                return Unauthorized("Token JWT não fornecido");
            }

            var response = getUser.Execute(jwt);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}