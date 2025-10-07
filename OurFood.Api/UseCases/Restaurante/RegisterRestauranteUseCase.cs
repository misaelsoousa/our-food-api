using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using OurFood.Api.Infrastructure;
using OurFood.Api.Services;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Restaurante;

public interface IRegisterRestauranteUseCase
{
    Task<ResponseRestaurante> Execute(RequestRestaurante request, IFormFile? imagemFile);
}

public class RegisterRestauranteUseCase(OurFoodDbContext db, IS3Service s3Service) : IRegisterRestauranteUseCase
{
    public async Task<ResponseRestaurante> Execute(RequestRestaurante request, IFormFile? imagemFile)
    {
        string? caminhoImagem = null;
        if (imagemFile != null && imagemFile.Length > 0)
        {
            caminhoImagem = await s3Service.UploadFileAsync(imagemFile, "restaurantes");
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
            !string.IsNullOrEmpty(entity.Imagem) ? s3Service.GetFileUrl(entity.Imagem) : entity.Imagem
        );
    }
}
