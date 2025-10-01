namespace OurFood.Communication.Responses;

using System.Collections.Generic;

public record ResponseRestauranteDetalhe(int Id, string Nome, string? Imagem, List<ResponseProduto> Produtos);






