using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Restaurante;

public interface IUpdateRestauranteUseCase
{
    (ResponseRestaurante? response, string? error) Execute(int id, RequestUpdateRestaurante request, IFormFile? imagemFile);
}

public class UpdateRestauranteUseCase(OurFoodDbContext db) : IUpdateRestauranteUseCase
{
    public (ResponseRestaurante? response, string? error) Execute(int id, RequestUpdateRestaurante request, IFormFile? imagemFile)
    {
        try
        {
            // Buscar o restaurante existente
            var restaurante = db.Restaurantes.FirstOrDefault(r => r.Id == id);
            if (restaurante == null)
                return (null, "Restaurante não encontrado");

            // Atualizar os dados do restaurante
            restaurante.Nome = request.Nome;

            // Se uma nova imagem foi enviada, fazer upload
            if (imagemFile != null && imagemFile.Length > 0)
            {
                var caminhoImagem = SalvarImagem(imagemFile);
                if (string.IsNullOrEmpty(caminhoImagem))
                    return (null, "Erro ao fazer upload da imagem");
                
                restaurante.Imagem = caminhoImagem;
            }

            // Salvar as alterações
            db.SaveChanges();

            // Retornar o restaurante atualizado
            return (new ResponseRestaurante(
                restaurante.Id,
                restaurante.Nome,
                restaurante.Imagem
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
            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens", "restaurantes");
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
            return Path.Combine("imagens", "restaurantes", nomeArquivo).Replace("\\", "/");
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
