using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Categoria;

public interface IUpdateCategoriaUseCase
{
    (ResponseCategoria? response, string? error) Execute(int id, RequestUpdateCategoria request, IFormFile? imagemFile);
}

public class UpdateCategoriaUseCase(OurFoodDbContext db) : IUpdateCategoriaUseCase
{
    public (ResponseCategoria? response, string? error) Execute(int id, RequestUpdateCategoria request, IFormFile? imagemFile)
    {
        try
        {
            // Buscar a categoria existente
            var categoria = db.Categorias.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
                return (null, "Categoria não encontrada");

            // Atualizar os dados da categoria
            categoria.Nome = request.Nome;
            categoria.CorHex = request.CorHex;

            // Se uma nova imagem foi enviada, fazer upload
            if (imagemFile != null && imagemFile.Length > 0)
            {
                var caminhoImagem = SalvarImagem(imagemFile);
                if (string.IsNullOrEmpty(caminhoImagem))
                    return (null, "Erro ao fazer upload da imagem");
                
                categoria.Imagem = caminhoImagem;
            }

            // Salvar as alterações
            db.SaveChanges();

            // Retornar a categoria atualizada
            return (new ResponseCategoria(
                categoria.Id,
                categoria.Nome,
                categoria.CorHex,
                categoria.Imagem
            ), null);
        }
        catch (Exception ex)
        {
            return (null, $"Erro interno: {ex.Message}");
        }
    }

    private string? SalvarImagem(IFormFile file)
    {
        try
        {
            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens", "categorias");
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
            return Path.Combine("imagens", "categorias", nomeArquivo).Replace("\\", "/");
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
