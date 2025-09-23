using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OurFood.Api.UseCases.Auth;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ILoginUseCase loginUseCase, IRegisterUsuarioUseCase registerUsuarioUseCase)
    : ControllerBase
{
    private string GenerateToken(int userId)
    {
        var config = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var issuer = config["Jwt:Issuer"];
        var audience = config["Jwt:Audience"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(int.Parse(config["Jwt:ExpiresMinutes"] ?? "120"));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) },
            expires: expires,
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ResponseAuth), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Register([FromBody] RequestAuth request)
    {
        try
        {
            var token = GenerateToken(0);
            var response = registerUsuarioUseCase.Execute(request, token);
            return Created(string.Empty, response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseAuth), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Login([FromBody] RequestLogin request)
    {
        var (response, error) = loginUseCase.Execute(request, GenerateToken);
        if (error != null) return BadRequest(error);
        return Ok(response);
    }
}
