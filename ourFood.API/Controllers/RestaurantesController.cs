using Microsoft.AspNetCore.Mvc;
using ourFood.API.useCases.Restaurante;
using ourFood.Communication.Requests;
using ourFood.Communication.Responses;

namespace ourFood.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantesController : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(typeof(ResponseAllRestaurantes), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public IActionResult GetAll()
	{
		var response = new GetAllRestaurantes().Execute();
		if (response.Restaurantes.Count == 0) return NoContent();
		return Ok(response);
	}

	[HttpPost]
	[ProducesResponseType(typeof(ResponseRestaurante), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public IActionResult Create([FromForm] RequestRestaurante request, IFormFile? imagem)
	{
		var response = new RegisterRestauranteUseCase().Execute(request, imagem);
		return Created(string.Empty, response);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult Delete(int id)
	{
		var ok = new DeleteRestauranteUseCase().Execute(id);
		if (!ok) return NotFound();
		return NoContent();
	}

	[HttpGet("{id}")]
	[ProducesResponseType(typeof(ResponseRestauranteDetalhe), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult Detalhe(int id)
	{
		var (response, error) = new GetRestauranteDetalhe().Execute(id);
		if (error != null) return NotFound(error);
		return Ok(response);
	}
}


