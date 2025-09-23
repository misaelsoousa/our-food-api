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
            entity.Property(p => p.Preco).HasColumnName("preco");
            entity.Property(p => p.CategoriaId).HasColumnName("categoria_id");
            entity.HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaId)
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
    }
}
