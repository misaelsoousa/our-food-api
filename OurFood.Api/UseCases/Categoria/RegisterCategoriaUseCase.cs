using System;
using System.IO;
using OurFood.Api.Entities;
using Microsoft.AspNetCore.Http;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Categoria;

public interface IRegisterCategoriaUseCase
{
    ResponseCategoria Execute(RequestCategoria request, Microsoft.AspNetCore.Http.IFormFile imagemFile);
}

public class RegisterCategoriaUseCase(OurFoodDbContext db) : IRegisterCategoriaUseCase
{
    public ResponseCategoria Execute(RequestCategoria request, IFormFile? imagemFile)
    {
        string? caminhoDoArquivo = null;
        if (imagemFile != null && imagemFile.Length > 0)
        {
            caminhoDoArquivo = SalvarImagem(imagemFile);
        }

        var entity = new Entities.Categoria
        {
            Nome = request.Nome,
            CorHex = request.CorHex,
            Imagem = caminhoDoArquivo
        };

        db.Categorias.Add(entity);
        db.SaveChanges();

        return new ResponseCategoria
        (
            entity.Id,
            entity.Nome,
            entity.CorHex,
            entity.Imagem
        );
    }

    private string SalvarImagem(IFormFile file)
    {
        var pastaDeDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens", "categorias");
        if (!Directory.Exists(pastaDeDestino))
        {
            Directory.CreateDirectory(pastaDeDestino);
        }

        var extensao = Path.GetExtension(file.FileName);
        var nomeDoArquivo = Guid.NewGuid() + extensao;
        var caminhoCompleto = Path.Combine(pastaDeDestino, nomeDoArquivo);

        using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return Path.Combine("imagens", "categorias", nomeDoArquivo).Replace("\\", "/");
    }
}