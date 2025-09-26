using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Produto;

public interface IRegisterProdutoUseCase
{
    (ResponseProduto? response, string? error) Execute(RequestProduto request, IFormFile? imagemFile);
}

public class RegisterProdutoUseCase(OurFoodDbContext db) : IRegisterProdutoUseCase
{
    public (ResponseProduto? response, string? error) Execute(RequestProduto request, IFormFile? imagemFile)
    {
        var categoria = db.Categorias.FirstOrDefault(c => c.Id == request.CategoriaId);
        if (categoria == null) return (null, "Categoria não encontrada");

        string? caminhoImagem = null;
        if (imagemFile != null && imagemFile.Length > 0)
        {
            caminhoImagem = SalvarImagem(imagemFile);
        }

        var entity = new Entities.Produto
        {
            Nome = request.Nome,
            Imagem = caminhoImagem,
            Preco = request.Preco,
            Descricao = request.Descricao,
            CategoriaId = request.CategoriaId
        };

        db.Produtos.Add(entity);
        db.SaveChanges();

        return (new ResponseProduto
        (
            entity.Id,
            entity.Nome,
            entity.Imagem,
            entity.Preco,
            categoria.Id,
            entity.Descricao,
            categoria.Nome
        ), null);
    }

    private string? SalvarImagem(IFormFile file)
    {
        try
        {
            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens", "produtos");
            // Cria a pasta se não existir
            if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta); 

            var extensao = Path.GetExtension(file.FileName);
            var nomeArquivo = Guid.NewGuid() + extensao;
            var caminho = Path.Combine(pasta, nomeArquivo);

            // Garante que o recurso de arquivo seja liberado após o uso
            using (var stream = new FileStream(caminho, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        
            // Retorna o caminho relativo (com barras '/' para URL)
            return Path.Combine("imagens", "produtos", nomeArquivo).Replace("\\", "/");
        }
        catch (Exception ex)
        {
            // Aqui, você deve logar a exceção 'ex' para debug
            Console.WriteLine($"Erro ao salvar imagem: {ex.Message}");
            // Retorna null ou lança uma exceção customizada para ser tratada no UseCase
            throw new IOException("Falha ao salvar a imagem no disco. Verifique as permissões de pasta.", ex); 
        }
    }
}
