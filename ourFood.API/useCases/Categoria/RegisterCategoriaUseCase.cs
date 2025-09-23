using ourFood.API.Entitys;
using ourFood.API.Infrastructure;
using ourFood.Communication.Requests;
using ourFood.Communication.Responses;
using Microsoft.AspNetCore.Http; 

namespace ourFood.API.useCases.Categoria;

public class RegisterCategoriaUseCase
{
    public ResponseCategoria Execute(RequestCategoria request, IFormFile imagemFile)
    {
        var dbContext = new ourFoodDbContext();
        string? caminhoDoArquivo = null;
        if (imagemFile != null && imagemFile.Length > 0)
        {
            caminhoDoArquivo = SalvarImagem(imagemFile);
        }

        var entity = new Entitys.Categoria
        {
            Nome = request.Nome,
            Cor_hex = request.Cor_hex,
            Imagem = caminhoDoArquivo
        };

        dbContext.Categorias.Add(entity);
        dbContext.SaveChanges();

        return new ResponseCategoria
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Cor_hex = entity.Cor_hex,
            Imagem = entity.Imagem
        };
    }

    private string SalvarImagem(IFormFile file)
    {
        var pastaDeDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens", "categorias");
        if (!Directory.Exists(pastaDeDestino))
        {
            Directory.CreateDirectory(pastaDeDestino);
        }

        var extensao = Path.GetExtension(file.FileName);
        var nomeDoArquivo = Guid.NewGuid().ToString() + extensao;
        var caminhoCompleto = Path.Combine(pastaDeDestino, nomeDoArquivo);

        using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return Path.Combine("imagens", "categorias", nomeDoArquivo).Replace("\\", "/");
    }
}