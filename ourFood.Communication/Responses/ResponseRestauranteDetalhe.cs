namespace ourFood.Communication.Responses;

public class ResponseRestauranteDetalhe
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public string? Imagem { get; set; }
	public List<ResponseProduto> Produtos { get; set; } = new List<ResponseProduto>();
}




