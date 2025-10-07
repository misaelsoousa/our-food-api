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
    IUpdateCategoriaUseCase updateCategoriaUseCase,
    IUpdateCategoriaImagemUseCase updateCategoriaImagemUseCase,
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
    public async Task<IActionResult> Register([FromForm] RequestCategoria request, IFormFile? imagem)
    {
        if (imagem == null) return BadRequest("Imagem obrigatória");
        var response = await registerCategoriaUseCase.Execute(request, imagem);
        return Created(string.Empty, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseCategoria), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromForm] RequestUpdateCategoria request, IFormFile? imagem)
    {
        var (response, error) = await updateCategoriaUseCase.Execute(id, request, imagem);
        if (error != null) return BadRequest(error);
        if (response == null) return NotFound("Categoria não encontrada");
        return Ok(response);
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

    [HttpPatch("{id}/imagem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateImagem(int id, IFormFile imagem)
    {
        if (imagem == null || imagem.Length == 0)
            return BadRequest("Imagem é obrigatória");

        var (success, error, imageUrl) = await updateCategoriaImagemUseCase.Execute(id, imagem);
        
        if (!success)
        {
            if (error == "Categoria não encontrada")
                return NotFound(error);
            return BadRequest(error);
        }

        return Ok(new { imageUrl });
    }
}
