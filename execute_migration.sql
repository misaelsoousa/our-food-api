-- Script para adicionar campo restauranteId na tabela produtos
-- Execute este script no seu banco de dados MySQL

-- 1. Adicionar a coluna restaurante_id
ALTER TABLE produtos 
ADD COLUMN restaurante_id INT NOT NULL DEFAULT 3;

-- 2. Adicionar foreign key constraint
ALTER TABLE produtos 
ADD CONSTRAINT FK_produtos_restaurante 
FOREIGN KEY (restaurante_id) REFERENCES restaurantes(id);

-- 3. Criar índice para melhor performance
CREATE INDEX IX_produtos_restaurante_id ON produtos(restaurante_id);

-- 4. Verificar se a alteração foi aplicada
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'produtos' 
AND COLUMN_NAME = 'restaurante_id';


