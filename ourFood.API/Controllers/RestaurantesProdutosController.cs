using Microsoft.AspNetCore.Mvc;
using ourFood.API.useCases.RestauranteProduto;
using ourFood.Communication.Requests;

namespace ourFood.API.Controllers;

[Route("api/restaurantes-produtos")]
[ApiController]
public class RestaurantesProdutosController : ControllerBase
{
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public IActionResult Add(RequestRelacaoRestauranteProduto request)
	{
		var (ok, error) = new AddRestauranteProdutoUseCase().Execute(request);
		if (!ok) return BadRequest(error);
		return NoContent();
	}

	[HttpDelete]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public IActionResult Remove([FromBody] RequestRelacaoRestauranteProduto request)
	{
		var (ok, error) = new RemoveRestauranteProdutoUseCase().Execute(request);
		if (!ok) return BadRequest(error);
		return NoContent();
	}
}




