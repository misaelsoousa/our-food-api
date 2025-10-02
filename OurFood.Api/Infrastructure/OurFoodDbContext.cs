using Microsoft.EntityFrameworkCore;
using OurFood.Api.Entities;

namespace OurFood.Api.Infrastructure;

public class OurFoodDbContext(DbContextOptions<OurFoodDbContext> options) : DbContext(options)
{
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Restaurante> Restaurantes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<RestauranteProduto> RestaurantesProdutos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoItem> PedidoItens { get; set; }
    public DbSet<ProdutoFavorito> ProdutoFavoritos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // usuarios
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("id");
            entity.Property(u => u.Email).HasColumnName("email").IsRequired();
            entity.Property(u => u.Nome).HasColumnName("nome").IsRequired();
            entity.Property(u => u.SenhaHash).HasColumnName("senha_hash").IsRequired();
        });

        // categorias
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("id");
            entity.Property(c => c.Nome).HasColumnName("nome").IsRequired();
            entity.Property(c => c.CorHex).HasColumnName("cor_hex").IsRequired();
            entity.Property(c => c.Imagem).HasColumnName("imagem");
        });

        // restaurantes
        modelBuilder.Entity<Restaurante>(entity =>
        {
            entity.ToTable("restaurantes");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).HasColumnName("id");
            entity.Property(r => r.Nome).HasColumnName("nome").IsRequired();
            entity.Property(r => r.Imagem).HasColumnName("imagem");
        });

        // produtos
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("produtos");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.Nome).HasColumnName("nome").IsRequired();
            entity.Property(p => p.Imagem).HasColumnName("imagem");
            entity.Property(p => p.Descricao).HasColumnName("descricao");
            entity.Property(p => p.Preco).HasColumnName("preco");
            entity.Property(p => p.CategoriaId).HasColumnName("categoria_id");
            entity.Property(p => p.RestauranteId).HasColumnName("restaurante_id").IsRequired();
            
            entity.HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(p => p.Restaurante)
                .WithMany()
                .HasForeignKey(p => p.RestauranteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // restaurantes_produtos (join)
        modelBuilder.Entity<RestauranteProduto>(entity =>
        {
            entity.ToTable("restaurantes_produtos");
            entity.HasKey(rp => new { rp.RestauranteId, rp.ProdutoId });
            entity.Property(rp => rp.RestauranteId).HasColumnName("restaurante_id");
            entity.Property(rp => rp.ProdutoId).HasColumnName("produto_id");
            entity.HasOne(rp => rp.Restaurante)
                .WithMany(r => r.RestauranteProdutos)
                .HasForeignKey(rp => rp.RestauranteId);
            entity.HasOne(rp => rp.Produto)
                .WithMany(p => p.RestauranteProdutos)
                .HasForeignKey(rp => rp.ProdutoId);
        });

        // pedidos
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable("pedidos");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.UsuarioId).HasColumnName("usuario_id").IsRequired();
            entity.Property(p => p.RestauranteId).HasColumnName("restaurante_id").IsRequired();
            entity.Property(p => p.Status).HasColumnName("status").IsRequired();
            entity.Property(p => p.DataPedido).HasColumnName("data_pedido").IsRequired();
            entity.Property(p => p.DataEntrega).HasColumnName("data_entrega");
            entity.Property(p => p.ValorTotal).HasColumnName("valor_total").IsRequired();
            entity.Property(p => p.TaxaEntrega).HasColumnName("taxa_entrega");
            entity.Property(p => p.ValorFinal).HasColumnName("valor_final").IsRequired();
            entity.Property(p => p.EnderecoEntrega).HasColumnName("endereco_entrega").IsRequired();
            entity.Property(p => p.Observacoes).HasColumnName("observacoes");
            entity.Property(p => p.MetodoPagamento).HasColumnName("metodo_pagamento").IsRequired();
            entity.Property(p => p.Avaliacao).HasColumnName("avaliacao");
            entity.Property(p => p.ComentarioAvaliacao).HasColumnName("comentario_avaliacao");
            entity.Property(p => p.CreatedAt).HasColumnName("created_at");
            entity.Property(p => p.UpdatedAt).HasColumnName("updated_at");
            
            entity.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(p => p.Restaurante)
                .WithMany()
                .HasForeignKey(p => p.RestauranteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // pedido_itens
        modelBuilder.Entity<PedidoItem>(entity =>
        {
            entity.ToTable("pedido_itens");
            entity.HasKey(pi => pi.Id);
            entity.Property(pi => pi.Id).HasColumnName("id");
            entity.Property(pi => pi.PedidoId).HasColumnName("pedido_id").IsRequired();
            entity.Property(pi => pi.ProdutoId).HasColumnName("produto_id").IsRequired();
            entity.Property(pi => pi.Quantidade).HasColumnName("quantidade").IsRequired();
            entity.Property(pi => pi.PrecoUnitario).HasColumnName("preco_unitario").IsRequired();
            entity.Property(pi => pi.PrecoTotal).HasColumnName("preco_total").IsRequired();
            entity.Property(pi => pi.ObservacoesItem).HasColumnName("observacoes_item");
            
            entity.HasOne(pi => pi.Pedido)
                .WithMany(p => p.PedidoItens)
                .HasForeignKey(pi => pi.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(pi => pi.Produto)
                .WithMany()
                .HasForeignKey(pi => pi.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // pedidos
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable("pedidos");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.UsuarioId).HasColumnName("usuario_id").IsRequired();
            entity.Property(p => p.RestauranteId).HasColumnName("restaurante_id").IsRequired();
            entity.Property(p => p.Status).HasColumnName("status").IsRequired();
            entity.Property(p => p.DataPedido).HasColumnName("data_pedido").IsRequired();
            entity.Property(p => p.DataEntrega).HasColumnName("data_entrega");
            entity.Property(p => p.ValorTotal).HasColumnName("valor_total").IsRequired();
            entity.Property(p => p.TaxaEntrega).HasColumnName("taxa_entrega");
            entity.Property(p => p.ValorFinal).HasColumnName("valor_final").IsRequired();
            entity.Property(p => p.EnderecoEntrega).HasColumnName("endereco_entrega").IsRequired();
            entity.Property(p => p.Observacoes).HasColumnName("observacoes");
            entity.Property(p => p.MetodoPagamento).HasColumnName("metodo_pagamento").IsRequired();
            entity.Property(p => p.Avaliacao).HasColumnName("avaliacao");
            entity.Property(p => p.ComentarioAvaliacao).HasColumnName("comentario_avaliacao");
            entity.Property(p => p.CreatedAt).HasColumnName("created_at");
            entity.Property(p => p.UpdatedAt).HasColumnName("updated_at");
            
            entity.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(p => p.Restaurante)
                .WithMany()
                .HasForeignKey(p => p.RestauranteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // pedido_itens
        modelBuilder.Entity<PedidoItem>(entity =>
        {
            entity.ToTable("pedido_itens");
            entity.HasKey(pi => pi.Id);
            entity.Property(pi => pi.Id).HasColumnName("id");
            entity.Property(pi => pi.PedidoId).HasColumnName("pedido_id").IsRequired();
            entity.Property(pi => pi.ProdutoId).HasColumnName("produto_id").IsRequired();
            entity.Property(pi => pi.Quantidade).HasColumnName("quantidade").IsRequired();
            entity.Property(pi => pi.PrecoUnitario).HasColumnName("preco_unitario").IsRequired();
            entity.Property(pi => pi.PrecoTotal).HasColumnName("preco_total").IsRequired();
            entity.Property(pi => pi.ObservacoesItem).HasColumnName("observacoes_item");
            
            entity.HasOne(pi => pi.Pedido)
                .WithMany(p => p.PedidoItens)
                .HasForeignKey(pi => pi.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(pi => pi.Produto)
                .WithMany()
                .HasForeignKey(pi => pi.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // produto_favoritos
        modelBuilder.Entity<ProdutoFavorito>(entity =>
        {
            entity.ToTable("produto_favoritos");
            entity.HasKey(pf => pf.Id);
            entity.Property(pf => pf.Id).HasColumnName("id");
            entity.Property(pf => pf.UsuarioId).HasColumnName("usuario_id").IsRequired();
            entity.Property(pf => pf.ProdutoId).HasColumnName("produto_id").IsRequired();
            entity.Property(pf => pf.CreatedAt).HasColumnName("created_at").IsRequired();
            
            entity.HasIndex(pf => new { pf.UsuarioId, pf.ProdutoId }).IsUnique();
            
            entity.HasOne(pf => pf.Usuario)
                .WithMany()
                .HasForeignKey(pf => pf.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(pf => pf.Produto)
                .WithMany()
                .HasForeignKey(pf => pf.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}