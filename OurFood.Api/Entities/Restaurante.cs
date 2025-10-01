using System.Collections.Generic;

namespace OurFood.Api.Entities;

public class Restaurante
{
	public int Id { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string? Imagem { get; set; }

	public ICollection<RestauranteProduto> RestauranteProdutos { get; set; } = new List<RestauranteProduto>();
}


