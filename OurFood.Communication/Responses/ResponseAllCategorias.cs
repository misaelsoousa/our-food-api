using System.Collections.Generic;

namespace OurFood.Communication.Responses;

public record ResponseAllCategorias (
    List<ResponseCategoria> Categorias
);