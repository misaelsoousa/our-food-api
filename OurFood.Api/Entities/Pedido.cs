namespace OurFood.Api.Entities;

public class Pedido
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int RestauranteId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataPedido { get; set; }
    public DateTime? DataEntrega { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal TaxaEntrega { get; set; }
    public decimal ValorFinal { get; set; }
    public string EnderecoEntrega { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public string MetodoPagamento { get; set; } = string.Empty;
    public int? Avaliacao { get; set; }
    public string? ComentarioAvaliacao { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Usuario Usuario { get; set; } = null!;
    public Restaurante Restaurante { get; set; } = null!;
    public ICollection<PedidoItem> PedidoItens { get; set; } = new List<PedidoItem>();
}