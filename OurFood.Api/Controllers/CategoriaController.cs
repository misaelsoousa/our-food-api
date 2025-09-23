using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurFood.Api.UseCases.Categoria;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriaController(
    IGetAllCategorias getAllCategorias,
    IRegisterCategoriaUseCase registerCategoriaUseCase,
    IDeleteCategoriaUseCase deleteCategoriaUseCase)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseAllCategorias), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetAll()
    {
        var response = getAllCategorias.Execute();
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
        if (imagem == null) return BadRequest("Imagem obrigatória");
        var response = registerCategoriaUseCase.Execute(request, imagem);
        return Created(string.Empty, response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Delete(int id)
    {
        var ok = deleteCategoriaUseCase.Execute(id);
        if (!ok) return BadRequest("Categoria não encontrada");
        return NoContent();
    }
}
