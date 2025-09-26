using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurFood.Api.UseCases.Produto;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController(
    IGetAllProdutos getAllProdutos,
    IRegisterProdutoUseCase registerProdutoUseCase,
    IDeleteProdutoUseCase deleteProdutoUseCase,
    IGetByIdUseCase getByIdUseCase,
    IToggleFavoritoUseCase ToggleFavoritoUseCase)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseAllProdutos), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetAll()
    {
        var response = getAllProdutos.Execute();
        if (response.Produtos.Count == 0) return NoContent();
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseProduto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var response = getByIdUseCase.Execute(id);

        if (response is null)
        {
            return NotFound(); 
        }

        return Ok(response);
    }
    [HttpPost]
    [ProducesResponseType(typeof(ResponseProduto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromForm] RequestProduto request, IFormFile? imagem)
    {
        var (response, error) = registerProdutoUseCase.Execute(request, imagem);
        if (error != null) return BadRequest(error);
        return Created(string.Empty, response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Delete(int id)
    {
        var ok = deleteProdutoUseCase.Execute(id);
        if (!ok) return BadRequest("Produto não encontrado");
        return NoContent();
    }
    
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Favoritar(int id)
    {
        var (success, error) = ToggleFavoritoUseCase.Execute(id);

        if (!success && error == "Produto não encontrado.")
        {
            return NotFound(); // Retorna 404 se não achar o produto
        }
    
        if (!success)
        {
            return BadRequest(error); // Retorna 400 para outros erros
        }

        return NoContent();
    }
}
