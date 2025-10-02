using System.Collections.Generic;

namespace OurFood.Api.Entities;

public class Produto
{
	public int Id { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string? Imagem { get; set; }
	public string? Descricao { get; set; }
	public decimal Preco { get; set; }
	public int? CategoriaId { get; set; }
	public Categoria Categoria { get; set; } = null!;
	public int RestauranteId { get; set; }
	public Restaurante Restaurante { get; set; } = null!;

	public ICollection<RestauranteProduto> RestauranteProdutos { get; set; } = new List<RestauranteProduto>();
}


