namespace OurFood.Communication.Requests;

public record RequestProduto(string Nome, int CategoriaId, decimal Preco, string Descricao, int RestauranteId);
