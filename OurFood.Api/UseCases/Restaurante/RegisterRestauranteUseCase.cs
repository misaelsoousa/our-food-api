using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Restaurante;

public interface IRegisterRestauranteUseCase
{
    ResponseRestaurante Execute(RequestRestaurante request, IFormFile? imagemFile);
}

public class RegisterRestauranteUseCase(OurFoodDbContext db) : IRegisterRestauranteUseCase
{
    public ResponseRestaurante Execute(RequestRestaurante request, IFormFile? imagemFile)
    {
        string? caminhoImagem = null;
        if (imagemFile != null && imagemFile.Length > 0)
        {
            caminhoImagem = SalvarImagem(imagemFile);
        }

        var entity = new Entities.Restaurante
        {
            Nome = request.Nome,
            Imagem = caminhoImagem
        };

        db.Restaurantes.Add(entity);
        db.SaveChanges();

        return new ResponseRestaurante(
            entity.Id,
            entity.Nome,
            entity.Imagem
        );
    }

    private string SalvarImagem(IFormFile file)
    {
        var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens", "restaurantes");
        if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);
        var extensao = Path.GetExtension(file.FileName);
        var nomeArquivo = Guid.NewGuid() + extensao;
        var caminho = Path.Combine(pasta, nomeArquivo);
        using var stream = new FileStream(caminho, FileMode.Create);
        file.CopyTo(stream);
        return Path.Combine("imagens", "restaurantes", nomeArquivo).Replace("\\", "/");
    }
}
