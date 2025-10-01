namespace OurFood.Api.Entities;

public class ProdutoFavorito
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int ProdutoId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Usuario Usuario { get; set; } = null!;
    public Produto Produto { get; set; } = null!;
}
