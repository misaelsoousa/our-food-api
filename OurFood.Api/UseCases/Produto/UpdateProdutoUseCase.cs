using Microsoft.EntityFrameworkCore;
using OurFood.Api.Infrastructure;
using OurFood.Communication.Requests;
using OurFood.Communication.Responses;

namespace OurFood.Api.UseCases.Produto;

public interface IUpdateProdutoUseCase
{
    (ResponseProduto? response, string? error) Execute(int id, RequestUpdateProduto request, IFormFile? imagemFile);
}

public class UpdateProdutoUseCase(OurFoodDbContext db) : IUpdateProdutoUseCase
{
    public (ResponseProduto? response, string? error) Execute(int id, RequestUpdateProduto request, IFormFile? imagemFile)
    {
        try
        {
            // Buscar o produto existente
            var produto = db.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
                return (null, "Produto não encontrado");

            // Verificar se a categoria existe
            var categoria = db.Categorias.FirstOrDefault(c => c.Id == request.CategoriaId);
            if (categoria == null)
                return (null, "Categoria não encontrada");

            // Verificar se o restaurante existe
            var restaurante = db.Restaurantes.FirstOrDefault(r => r.Id == request.RestauranteId);
            if (restaurante == null)
                return (null, "Restaurante não encontrado");

            // Atualizar os dados do produto
            produto.Nome = request.Nome;
            produto.CategoriaId = request.CategoriaId;
            produto.Preco = request.Preco;
            produto.Descricao = request.Descricao;
            produto.RestauranteId = request.RestauranteId;

            // Se uma nova imagem foi enviada, fazer upload
            if (imagemFile != null && imagemFile.Length > 0)
            {
                var caminhoImagem = SalvarImagem(imagemFile);
                if (string.IsNullOrEmpty(caminhoImagem))
                    return (null, "Erro ao fazer upload da imagem");
                
                produto.Imagem = caminhoImagem;
            }

            // Salvar as alterações
            db.SaveChanges();

            // Retornar o produto atualizado
            return (new ResponseProduto(
                produto.Id,
                produto.Nome,
                produto.Imagem,
                produto.Preco,
                categoria.Id,
                categoria.Nome ?? string.Empty,
                produto.Descricao ?? string.Empty,
                restaurante.Id,
                restaurante.Nome
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
