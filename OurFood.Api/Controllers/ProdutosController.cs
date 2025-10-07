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
    IUpdateProdutoUseCase updateProdutoUseCase,
    IUpdateProdutoImagemUseCase updateProdutoImagemUseCase,
    IDeleteProdutoUseCase deleteProdutoUseCase,
    IGetByIdUseCase getByIdUseCase,
    IToggleFavoritoUseCase ToggleFavoritoUseCase,
    IGetFavoritosUseCase getFavoritosUseCase)
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
    public async Task<IActionResult> Create([FromForm] RequestProduto request, IFormFile? imagem)
    {
        var (response, error) = await registerProdutoUseCase.Execute(request, imagem);
        if (error != null) return BadRequest(error);
        return Created(string.Empty, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ResponseProduto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromForm] RequestUpdateProduto request, IFormFile? imagem)
    {
        var (response, error) = updateProdutoUseCase.Execute(id, request, imagem);
        if (error != null) return BadRequest(error);
        if (response == null) return NotFound("Produto não encontrado");
        return Ok(response);
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
    
    [HttpPatch("{id}/favoritar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Favoritar(int id)
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

            var (success, error, isFavorito) = ToggleFavoritoUseCase.Execute(id, jwt);

            if (!success && error == "Produto não encontrado.")
            {
                return NotFound(); // Retorna 404 se não achar o produto
            }
        
            if (!success)
            {
                return BadRequest(error); // Retorna 400 para outros erros
            }

            return Ok(new { favoritado = isFavorito });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("favoritos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetFavoritos()
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

            var response = getFavoritosUseCase.Execute(jwt);
            if (response.Produtos.Count == 0) return NoContent();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}/imagem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateImagem(int id, IFormFile imagem)
    {
        if (imagem == null || imagem.Length == 0)
            return BadRequest("Imagem é obrigatória");

        var (success, error, imageUrl) = await updateProdutoImagemUseCase.Execute(id, imagem);
        
        if (!success)
        {
            if (error == "Produto não encontrado")
                return NotFound(error);
            return BadRequest(error);
        }

        return Ok(new { imageUrl });
    }
}
