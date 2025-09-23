namespace ourFood.API.Entitys;

public class RestauranteProduto
{
	public int RestauranteId { get; set; }
	public Restaurante Restaurante { get; set; }

	public int ProdutoId { get; set; }
	public Produto Produto { get; set; }
}



