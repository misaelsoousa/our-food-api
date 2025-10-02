namespace OurFood.Communication.Responses;

public record ResponseProduto(int Id, string Nome, string? Imagem, decimal Preco, int CategoriaId, string CategoriaNome, string Descricao, int RestauranteId, string RestauranteNome);
