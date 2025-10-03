# Rotas de Edição - OurFood API

## ✅ Resumo das Rotas Criadas

Foram criadas rotas PUT para editar todas as entidades principais da API:

### 1. **Produtos** - `PUT /api/produtos/{id}`
- **UseCase**: `UpdateProdutoUseCase`
- **Request**: `RequestUpdateProduto` (FormData com imagem opcional)
- **Campos editáveis**:
  - Nome (obrigatório)
  - CategoriaId (obrigatório)
  - Preço (obrigatório)
  - Descrição (opcional)
  - RestauranteId (obrigatório)
  - Imagem (opcional - arquivo)

### 2. **Restaurantes** - `PUT /api/restaurantes/{id}`
- **UseCase**: `UpdateRestauranteUseCase`
- **Request**: `RequestUpdateRestaurante` (FormData com imagem opcional)
- **Campos editáveis**:
  - Nome (obrigatório)
  - Imagem (opcional - arquivo)

### 3. **Categorias** - `PUT /api/categoria/{id}`
- **UseCase**: `UpdateCategoriaUseCase`
- **Request**: `RequestUpdateCategoria` (JSON)
- **Campos editáveis**:
  - Nome (obrigatório)

### 4. **Pedidos** - `PUT /api/pedidos/{id}`
- **UseCase**: `UpdatePedidoUseCase`
- **Request**: `RequestUpdatePedido` (JSON)
- **Campos editáveis**:
  - Status (obrigatório)
  - Observações (opcional)
  - Método de Pagamento (obrigatório)
  - Endereço de Entrega (opcional)
  - Taxa de Entrega (opcional)
  - Valor Final (opcional)

## 📁 Arquivos Criados/Modificados

### DTOs de Request:
- `RequestUpdateProduto.cs`
- `RequestUpdateRestaurante.cs`
- `RequestUpdateCategoria.cs`
- `RequestUpdatePedido.cs`

### UseCases:
- `UpdateProdutoUseCase.cs`
- `UpdateRestauranteUseCase.cs`
- `UpdateCategoriaUseCase.cs`
- `UpdatePedidoUseCase.cs`

### Controllers Atualizados:
- `ProdutosController.cs` - Adicionada rota PUT
- `RestaurantesController.cs` - Adicionada rota PUT
- `CategoriaController.cs` - Adicionada rota PUT
- `PedidosController.cs` - Adicionada rota PUT

### Dependency Injection:
- `DependencyInjection.cs` - Registrados todos os novos UseCases

## 🔧 Exemplos de Uso

### Editar Produto:
```http
PUT /api/produtos/1
Content-Type: multipart/form-data

{
  "nome": "Pizza Margherita Atualizada",
  "categoriaId": 2,
  "preco": 35.90,
  "descricao": "Pizza com ingredientes frescos",
  "restauranteId": 1
}
```

### Editar Restaurante:
```http
PUT /api/restaurantes/1
Content-Type: multipart/form-data

{
  "nome": "Restaurante Atualizado"
}
```

### Editar Categoria:
```http
PUT /api/categoria/1
Content-Type: application/json

{
  "nome": "Pizzas Premium"
}
```

### Editar Pedido:
```http
PUT /api/pedidos/1
Content-Type: application/json
Authorization: Bearer {token}

{
  "status": "Em Preparo",
  "observacoes": "Sem cebola",
  "metodoPagamento": "Cartão",
  "enderecoEntrega": "Rua das Flores, 123",
  "taxaEntrega": 5.00,
  "valorFinal": 40.90
}
```

## ✅ Validações Implementadas

- **Campos obrigatórios**: Validação com `[Required]`
- **Valores numéricos**: Validação com `[Range]` para preços
- **Existência de entidades**: Verificação se categoria/restaurante existem
- **Autenticação**: Pedidos requerem token JWT
- **Tratamento de erros**: Try-catch com mensagens específicas

## 📊 Status Codes Retornados

- **200 OK**: Edição realizada com sucesso
- **400 Bad Request**: Dados inválidos ou erro na validação
- **404 Not Found**: Entidade não encontrada
- **401 Unauthorized**: Token JWT inválido (apenas para pedidos)

## 🎯 Observações Importantes

- ✅ **Compilação**: API compila sem erros
- ✅ **Estrutura**: Todas as rotas mantêm a mesma estrutura de resposta das rotas existentes
- ✅ **Upload**: Upload de imagem é opcional para produtos e restaurantes
- ✅ **Validações**: Validações são consistentes com as rotas de criação
- ✅ **Tratamento de erros**: Tratamento de erros padronizado em todos os UseCases
- ✅ **Dependency Injection**: Todos os UseCases registrados corretamente

## 🚀 Status Final

**✅ CONCLUÍDO COM SUCESSO!**

Todas as rotas de edição foram implementadas e estão funcionais:
- ✅ Produtos
- ✅ Restaurantes  
- ✅ Categorias
- ✅ Pedidos

A API está pronta para uso com todas as funcionalidades de edição implementadas!
