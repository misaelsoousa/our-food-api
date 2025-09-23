using Microsoft.AspNetCore.Mvc;
using ourFood.API.useCases.Produto;
using ourFood.Communication.Requests;
using ourFood.Communication.Responses;

namespace ourFood.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(typeof(ResponseAllProdutos), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public IActionResult GetAll()
	{
		var response = new GetAllProdutos().Execute();
		if (response.Produtos.Count == 0) return NoContent();
		return Ok(response);
	}

	[HttpPost]
	[ProducesResponseType(typeof(ResponseProduto), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public IActionResult Create([FromForm] RequestProduto request, IFormFile? imagem)
	{
		var (response, error) = new RegisterProdutoUseCase().Execute(request, imagem);
		if (error != null) return BadRequest(error);
		return Created(string.Empty, response);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult Delete(int id)
	{
		var ok = new DeleteProdutoUseCase().Execute(id);
		if (!ok) return NotFound();
		return NoContent();
	}
}


