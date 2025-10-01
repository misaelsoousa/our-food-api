namespace OurFood.Api.Entities;

public class Categoria
{
	public int Id { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string CorHex { get; set; } = string.Empty;
	public string? Imagem { get; set; }
}
