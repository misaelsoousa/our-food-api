-- Adicionar campo restauranteId na tabela produtos
ALTER TABLE produtos 
ADD COLUMN restauranteId INT NOT NULL DEFAULT 3;

-- Adicionar foreign key constraint
ALTER TABLE produtos 
ADD CONSTRAINT FK_produtos_restaurante 
FOREIGN KEY (restauranteId) REFERENCES restaurantes(id);

-- Criar índice para melhor performance
CREATE INDEX IX_produtos_restauranteId ON produtos(restauranteId);

-- Verificar se a alteração foi aplicada corretamente
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'produtos' 
AND COLUMN_NAME = 'restauranteId';


