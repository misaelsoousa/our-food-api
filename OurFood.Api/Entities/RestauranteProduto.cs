namespace OurFood.Api.Entities;

public class RestauranteProduto
{
	public int RestauranteId { get; set; }
	public Restaurante Restaurante { get; set; } = null!;

	public int ProdutoId { get; set; }
	public Produto Produto { get; set; } = null!;
}



