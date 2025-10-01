using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurFood.Api.UseCases.RestauranteProduto;
using OurFood.Communication.Requests;

namespace OurFood.Api.Controllers;

[Route("api/restaurantes-produtos")]
[ApiController]
public class RestaurantesProdutosController(
    IAddRestauranteProdutoUseCase addUseCase,
    IRemoveRestauranteProdutoUseCase removeUseCase)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Add(RequestRelacaoRestauranteProduto request)
    {
        var (ok, error) = addUseCase.Execute(request);
        if (!ok) return BadRequest(error);
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Remove([FromBody] RequestRelacaoRestauranteProduto request)
    {
        var (ok, error) = removeUseCase.Execute(request);
        if (!ok) return BadRequest(error);
        return NoContent();
    }
}




