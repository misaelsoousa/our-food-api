# Como executar a migração do banco de dados

## Opção 1: Executar SQL diretamente

1. **Abra o MySQL Workbench ou phpMyAdmin**
2. **Conecte ao seu banco de dados**
3. **Execute o seguinte SQL:**

```sql
-- Adicionar campo restauranteId na tabela produtos
ALTER TABLE produtos 
ADD COLUMN restaurante_id INT NOT NULL DEFAULT 3;

-- Adicionar foreign key constraint
ALTER TABLE produtos 
ADD CONSTRAINT FK_produtos_restaurante 
FOREIGN KEY (restaurante_id) REFERENCES restaurantes(id);

-- Criar índice para melhor performance
CREATE INDEX IX_produtos_restaurante_id ON produtos(restaurante_id);
```

## Opção 2: Usar linha de comando MySQL

```bash
mysql -u root -p your_database_name < execute_migration.sql
```

## Opção 3: Usar PowerShell (Windows)

```powershell
# Ajuste as variáveis conforme seu ambiente
$server = "localhost"
$database = "ourfood"
$username = "root"
$password = "sua_senha"

mysql -h $server -u $username -p$password $database -e "ALTER TABLE produtos ADD COLUMN restaurante_id INT NOT NULL DEFAULT 3;"
mysql -h $server -u $username -p$password $database -e "ALTER TABLE produtos ADD CONSTRAINT FK_produtos_restaurante FOREIGN KEY (restaurante_id) REFERENCES restaurantes(id);"
mysql -h $server -u $username -p$password $database -e "CREATE INDEX IX_produtos_restaurante_id ON produtos(restaurante_id);"
```

## Verificar se funcionou

Execute esta query para verificar:

```sql
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'produtos' 
AND COLUMN_NAME = 'restaurante_id';
```

Deve retornar:
- COLUMN_NAME: restaurante_id
- DATA_TYPE: int
- IS_NULLABLE: NO
- COLUMN_DEFAULT: 3

## Após executar a migração

1. **Reinicie a API**
2. **Teste os endpoints** para garantir que funcionam
3. **Verifique se os produtos retornam restauranteId e restauranteNome**


