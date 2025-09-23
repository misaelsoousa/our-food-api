using System.Collections.Generic;

namespace OurFood.Api.Entities;

public class Produto
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public string? Imagem { get; set; }
	public decimal Preco { get; set; }
	public int? CategoriaId { get; set; }
	public Categoria Categoria { get; set; }

	public ICollection<RestauranteProduto> RestauranteProdutos { get; set; } = new List<RestauranteProduto>();
}


