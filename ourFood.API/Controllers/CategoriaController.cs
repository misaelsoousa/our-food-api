using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ourFood.API.useCases.Categoria;
using ourFood.Communication.Requests;
using ourFood.Communication.Responses;

namespace ourFood.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseAllCategorias), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetAll()
    {
        var useCase = new GetAllCategorias();

        var response = useCase.Execute();
        if (response.Categorias.Count == 0)
        {
            return NoContent();
        }
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseCategoria), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Register([FromForm] RequestCategoria request, IFormFile? imagem)
    {
        var useCase = new RegisterCategoriaUseCase();

        var response = useCase.Execute(request, imagem);

        return Created(string.Empty, response);
    }

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult Delete(int id)
	{
		var ok = new DeleteCategoriaUseCase().Execute(id);
		if (!ok) return NotFound();
		return NoContent();
	}
}
