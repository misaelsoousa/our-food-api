-- Script SQL simples para adicionar restaurante_id à tabela produtos
-- Execute este script no seu banco MySQL da AWS

-- 1. Adicionar a coluna restaurante_id
ALTER TABLE produtos ADD COLUMN restaurante_id INT NOT NULL DEFAULT 3;

-- 2. Adicionar a chave estrangeira
ALTER TABLE produtos ADD CONSTRAINT fk_produtos_restaurante_id FOREIGN KEY (restaurante_id) REFERENCES restaurantes(id);

-- 3. Criar índice para performance
CREATE INDEX idx_produtos_restaurante_id ON produtos(restaurante_id);

-- 4. Verificar se foi criado corretamente
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'produtos' AND COLUMN_NAME = 'restaurante_id';

-- 5. Verificar alguns produtos
SELECT id, nome, restaurante_id FROM produtos LIMIT 5;
