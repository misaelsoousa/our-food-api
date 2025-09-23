namespace ourFood.API.Entitys;

public class Restaurante
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public string? Imagem { get; set; }

	public ICollection<RestauranteProduto> RestauranteProdutos { get; set; } = new List<RestauranteProduto>();
}


