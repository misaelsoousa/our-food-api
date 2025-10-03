using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurFood.Api.UseCases.Restaurante;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantesController(
    IGetAllRestaurantes getAllRestaurantes,
    IRegisterRestauranteUseCase registerRestauranteUseCase,
    IUpdateRestauranteUseCase updateRestauranteUseCase,
    IDeleteRestauranteUseCase deleteRestauranteUseCase,
    IGetRestauranteDetalhe getRestauranteDetalhe)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseAllRestaurantes), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetAll()
    {
        var response = getAllRestaurantes.Execute();
        if (response.Restaurantes.Count == 0) return NoContent();
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRestaurante), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromForm] RequestRestaurante request, IFormFile? imagem)
    {
        var response = registerRestauranteUseCase.Execute(request, imagem);
        return Created(string.Empty, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseRestaurante), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromForm] RequestUpdateRestaurante request, IFormFile? imagem)
    {
        var (response, error) = updateRestauranteUseCase.Execute(id, request, imagem);
        if (error != null) return BadRequest(error);
        if (response == null) return NotFound("Restaurante não encontrado");
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Delete(int id)
    {
        var ok = deleteRestauranteUseCase.Execute(id);
        if (!ok) return BadRequest("Restaurante não encontrado");
        return NoContent();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseRestauranteDetalhe), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetDetalhe(int id)
    {
        var (response, error) = getRestauranteDetalhe.Execute(id);
        if (error != null) return NotFound(error);
        return Ok(response);
    }
}
