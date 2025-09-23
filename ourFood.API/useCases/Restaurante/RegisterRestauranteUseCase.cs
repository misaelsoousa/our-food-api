using ourFood.API.Infrastructure;
using ourFood.Communication.Requests;
using ourFood.Communication.Responses;
using Microsoft.AspNetCore.Http;

namespace ourFood.API.useCases.Restaurante;

public class RegisterRestauranteUseCase
{
	public ResponseRestaurante Execute(RequestRestaurante request, IFormFile? imagemFile)
	{
		var db = new ourFoodDbContext();

		string? caminhoImagem = null;
		if (imagemFile != null && imagemFile.Length > 0)
		{
			caminhoImagem = SalvarImagem(imagemFile);
		}

		var entity = new API.Entitys.Restaurante
		{
			Nome = request.Nome,
			Imagem = caminhoImagem
		};

		db.Restaurantes.Add(entity);
		db.SaveChanges();

		return new ResponseRestaurante
		{
			Id = entity.Id,
			Nome = entity.Nome,
			Imagem = entity.Imagem
		};
	}

	private string SalvarImagem(IFormFile file)
	{
		var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens", "restaurantes");
		if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);
		var extensao = Path.GetExtension(file.FileName);
		var nomeArquivo = Guid.NewGuid().ToString() + extensao;
		var caminho = Path.Combine(pasta, nomeArquivo);
		using var stream = new FileStream(caminho, FileMode.Create);
		file.CopyTo(stream);
		return Path.Combine("imagens", "restaurantes", nomeArquivo).Replace("\\", "/");
	}
}


