using Microsoft.AspNetCore.Mvc;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;
using OurFood.Api.UseCases.Pedido;

namespace OurFood.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController(
    ICriarPedidoUseCase criarPedido,
    IGetPedidosUsuarioUseCase getPedidosUsuario,
    IGetPedidoDetalheUseCase getPedidoDetalhe,
    IUpdatePedidoUseCase updatePedido,
    IUpdatePedidoStatusUseCase updatePedidoStatus,
    IAvaliarPedidoUseCase avaliarPedido
) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CriarPedido([FromBody] RequestPedido request)
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

            var response = criarPedido.Execute(request, jwt);
            return CreatedAtAction(nameof(GetPedidoDetalhe), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetPedidosUsuario()
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

            var response = getPedidosUsuario.Execute(jwt);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetPedidoDetalhe(int id)
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

            var (response, error) = getPedidoDetalhe.Execute(id, jwt);
            if (error != null)
            {
                return NotFound(error);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdatePedidoStatus(int id, [FromBody] RequestUpdatePedidoStatus request)
    {
        try
        {
            var (success, error) = updatePedidoStatus.Execute(id, request);
            if (!success)
            {
                return BadRequest(error);
            }

            return Ok(new { message = "Status do pedido atualizado com sucesso." });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult UpdatePedido(int id, [FromBody] RequestUpdatePedido request)
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

            var (response, error) = updatePedido.Execute(id, request);
            if (error != null) return BadRequest(error);
            if (response == null) return NotFound("Pedido não encontrado");
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/avaliar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult AvaliarPedido(int id, [FromBody] RequestAvaliacaoPedido request)
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

            var (success, error) = avaliarPedido.Execute(id, request, jwt);
            if (!success)
            {
                return BadRequest(error);
            }

            return Ok(new { message = "Pedido avaliado com sucesso." });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}