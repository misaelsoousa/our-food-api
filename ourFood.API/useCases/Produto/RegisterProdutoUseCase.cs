using ourFood.API.Infrastructure;
using ourFood.Communication.Requests;
using ourFood.Communication.Responses;
using Microsoft.AspNetCore.Http;

namespace ourFood.API.useCases.Produto;

public class RegisterProdutoUseCase
{
	public (ResponseProduto? response, string? error) Execute(RequestProduto request, IFormFile? imagemFile)
	{
		var db = new ourFoodDbContext();
		var categoria = db.Categorias.FirstOrDefault(c => c.Id == request.CategoriaId);
		if (categoria == null) return (null, "Categoria não encontrada");

		string? caminhoImagem = null;
		if (imagemFile != null && imagemFile.Length > 0)
		{
			caminhoImagem = SalvarImagem(imagemFile);
		}

		var entity = new API.Entitys.Produto
		{
			Nome = request.Nome,
			Imagem = caminhoImagem,
			Preco = request.Preco,
			CategoriaId = request.CategoriaId
		};

		db.Produtos.Add(entity);
		db.SaveChanges();

		return (new ResponseProduto
		{
			Id = entity.Id,
			Nome = entity.Nome,
			Imagem = entity.Imagem,
			Preco = entity.Preco,
			CategoriaId = categoria.Id,
			CategoriaNome = categoria.Nome
		}, null);
	}

	private string SalvarImagem(IFormFile file)
	{
		var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens", "produtos");
		if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);
		var extensao = Path.GetExtension(file.FileName);
		var nomeArquivo = Guid.NewGuid().ToString() + extensao;
		var caminho = Path.Combine(pasta, nomeArquivo);
		using var stream = new FileStream(caminho, FileMode.Create);
		file.CopyTo(stream);
		return Path.Combine("imagens", "produtos", nomeArquivo).Replace("\\", "/");
	}
}


