namespace OurFood.Api.Entities;

public class PedidoItem
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal PrecoTotal { get; set; }
    public string? ObservacoesItem { get; set; }

    // Navigation properties
    public Pedido Pedido { get; set; } = null!;
    public Produto Produto { get; set; } = null!;
}