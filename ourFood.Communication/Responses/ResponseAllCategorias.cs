using ourFood.Communication.Requests;

namespace ourFood.Communication.Responses;

public class ResponseAllCategorias
{
    public List<ResponseCategoria> Categorias { get; set; } = new List<ResponseCategoria>();
}
