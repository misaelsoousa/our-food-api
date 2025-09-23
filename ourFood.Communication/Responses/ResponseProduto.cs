namespace ourFood.Communication.Responses;

public class ResponseProduto
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public string? Imagem { get; set; }
	public decimal Preco { get; set; }
	public int CategoriaId { get; set; }
	public string CategoriaNome { get; set; }
}


