using System;
using System.IO;
using OurFood.Api.Entities;
using Microsoft.AspNetCore.Http;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Categoria;

public interface IRegisterCategoriaUseCase
{
    Task<ResponseCategoria> Execute(RequestCategoria request, Microsoft.AspNetCore.Http.IFormFile imagemFile);
}

public class RegisterCategoriaUseCase(OurFoodDbContext db, IS3Service s3Service) : IRegisterCategoriaUseCase
{
    public async Task<ResponseCategoria> Execute(RequestCategoria request, IFormFile? imagemFile)
    {
        string? caminhoDoArquivo = null;
        if (imagemFile != null && imagemFile.Length > 0)
        {
            caminhoDoArquivo = await s3Service.UploadFileAsync(imagemFile, "categorias");
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
            !string.IsNullOrEmpty(entity.Imagem) ? s3Service.GetFileUrl(entity.Imagem) : entity.Imagem
        );
    }
}