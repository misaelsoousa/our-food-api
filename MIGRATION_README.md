# Migração do Banco de Dados - Adicionar restaurante_id

## Problema
A API está retornando erro 204 (No Content) porque está tentando acessar a coluna `restaurante_id` que ainda não existe na tabela `produtos` do banco de dados.

## Solução Temporária
Modifiquei os UseCases para usar valores padrão temporários até que a migração seja executada:
- `GetAllProdutos.cs` - Removido Include(p => p.Restaurante)
- `GetByIdUseCase.cs` - Removido Include(p => p.Restaurante)  
- `GetFavoritosUseCase.cs` - Removido Include(p => p.Restaurante)

Todos agora usam:
- `restauranteId = 3` (valor padrão)
- `restauranteNome = "Restaurante Padrão"`

## Como Executar a Migração

### Opção 1: Via MySQL Workbench ou phpMyAdmin
1. Conecte ao seu banco MySQL da AWS
2. Execute o arquivo `migrate_aws.sql`
3. Verifique se a coluna foi criada corretamente

### Opção 2: Via linha de comando MySQL
```bash
mysql -h [SEU_HOST_AWS] -u [SEU_USUARIO] -p[SUA_SENHA] [SEU_BANCO] < migrate_aws.sql
```

### Opção 3: Via script PowerShell (se tiver acesso local)
```powershell
.\execute_migration.ps1
```

## Após a Migração
1. Reverta as mudanças temporárias nos UseCases
2. Restaure os `Include(p => p.Restaurante)` 
3. Use `p.RestauranteId` e `p.Restaurante.Nome` em vez dos valores padrão
4. Teste a API novamente

## Verificação
Após executar a migração, teste:
- GET /api/produtos - deve retornar produtos com restaurante_id
- GET /api/produtos/{id} - deve retornar produto específico
- GET /api/produtos/favoritos - deve retornar favoritos

## Arquivos Modificados Temporariamente
- `GetAllProdutos.cs`
- `GetByIdUseCase.cs` 
- `GetFavoritosUseCase.cs`

## Arquivo de Migração
- `migrate_aws.sql` - Script SQL para executar no banco AWS

