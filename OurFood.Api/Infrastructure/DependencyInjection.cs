using Microsoft.Extensions.DependencyInjection;
using OurFood.Api.UseCases.Auth;
using OurFood.Api.UseCases.Categoria;
using OurFood.Api.UseCases.Produto;
using OurFood.Api.UseCases.Restaurante;
using OurFood.Api.UseCases.RestauranteProduto;

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
        services.AddScoped<IDeleteCategoriaUseCase, DeleteCategoriaUseCase>();
        // Produto
        services.AddScoped<IGetAllProdutos, GetAllProdutos>();
        services.AddScoped<IRegisterProdutoUseCase, RegisterProdutoUseCase>();
        services.AddScoped<IDeleteProdutoUseCase, DeleteProdutoUseCase>();
        // Restaurante
        services.AddScoped<IGetAllRestaurantes, GetAllRestaurantes>();
        services.AddScoped<IRegisterRestauranteUseCase, RegisterRestauranteUseCase>();
        services.AddScoped<IDeleteRestauranteUseCase, DeleteRestauranteUseCase>();
        services.AddScoped<IGetRestauranteDetalhe, GetRestauranteDetalhe>();
        // RestauranteProduto
        services.AddScoped<IAddRestauranteProdutoUseCase, AddRestauranteProdutoUseCase>();
        services.AddScoped<IRemoveRestauranteProdutoUseCase, RemoveRestauranteProdutoUseCase>();
    }
}
