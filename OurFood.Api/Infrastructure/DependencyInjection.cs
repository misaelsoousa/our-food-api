using Microsoft.Extensions.DependencyInjection;
using OurFood.Api.Services;
using OurFood.Api.UseCases.Auth;
using OurFood.Api.UseCases.Categoria;
using OurFood.Api.UseCases.Pedido;
using OurFood.Api.UseCases.Produto;
using OurFood.Api.UseCases.Restaurante;
using OurFood.Api.UseCases.RestauranteProduto;
using OurFood.Api.UseCases.User;

namespace OurFood.Api.Infrastructure;

public static class DependencyInjection
{
    public static void AddUseCases(this IServiceCollection services)
    {
        // Auth
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IRegisterUsuarioUseCase, RegisterUsuarioUseCase>();
        // Categoria
        services.AddScoped<IGetAllCategorias, GetAllCategorias>();
        services.AddScoped<IRegisterCategoriaUseCase, RegisterCategoriaUseCase>();
        services.AddScoped<IUpdateCategoriaUseCase, UpdateCategoriaUseCase>();
        services.AddScoped<IDeleteCategoriaUseCase, DeleteCategoriaUseCase>();
        // Produto
        services.AddScoped<IGetAllProdutos, GetAllProdutos>();
        services.AddScoped<IRegisterProdutoUseCase, RegisterProdutoUseCase>();
        services.AddScoped<IUpdateProdutoUseCase, UpdateProdutoUseCase>();
        services.AddScoped<IDeleteProdutoUseCase, DeleteProdutoUseCase>();
        services.AddScoped<IGetByIdUseCase, GetByIdUseCase>();
        services.AddScoped<IToggleFavoritoUseCase, ToggleFavoritoUseCase>();
        services.AddScoped<IGetFavoritosUseCase, GetFavoritosUseCase>();
        // Restaurante
        services.AddScoped<IGetAllRestaurantes, GetAllRestaurantes>();
        services.AddScoped<IRegisterRestauranteUseCase, RegisterRestauranteUseCase>();
        services.AddScoped<IUpdateRestauranteUseCase, UpdateRestauranteUseCase>();
        services.AddScoped<IDeleteRestauranteUseCase, DeleteRestauranteUseCase>();
        services.AddScoped<IGetRestauranteDetalhe, GetRestauranteDetalhe>();
        // RestauranteProduto
        services.AddScoped<IAddRestauranteProdutoUseCase, AddRestauranteProdutoUseCase>();
        services.AddScoped<IRemoveRestauranteProdutoUseCase, RemoveRestauranteProdutoUseCase>();
        // Pedido
        services.AddScoped<ICriarPedidoUseCase, CriarPedidoUseCase>();
        services.AddScoped<IGetPedidosUsuarioUseCase, GetPedidosUsuarioUseCase>();
        services.AddScoped<IGetPedidoDetalheUseCase, GetPedidoDetalheUseCase>();
        services.AddScoped<IUpdatePedidoUseCase, UpdatePedidoUseCase>();
        services.AddScoped<IUpdatePedidoStatusUseCase, UpdatePedidoStatusUseCase>();
        services.AddScoped<IAvaliarPedidoUseCase, AvaliarPedidoUseCase>();
        // User
        services.AddScoped<IGetUser, GetUser>();
        // JWT
        services.AddScoped<IJwtService, JwtService>();
    }
}
