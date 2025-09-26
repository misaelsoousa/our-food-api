-- Create database if not exists
CREATE DATABASE IF NOT EXISTS ourfooddb;
USE ourfooddb;

-- ---------------------------------------------------------------------------------------------------
-- categorias table
-- ---------------------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS categorias (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    cor_hex VARCHAR(7) NOT NULL,
    imagem VARCHAR(255)
);

-- ---------------------------------------------------------------------------------------------------
-- restaurantes table
-- ---------------------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS restaurantes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    imagem VARCHAR(255)
);

-- ---------------------------------------------------------------------------------------------------
-- produtos table (ATUALIZADA com 'descricao' e ON DELETE CASCADE)
-- ---------------------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS produtos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    descricao VARCHAR(255) NULL, -- Novo campo adicionado (pode ser nulo)
    imagem VARCHAR(255),
    preco DECIMAL(10,2),
    categoria_id INT,
    FOREIGN KEY (categoria_id)
        REFERENCES categorias(id)
        ON DELETE CASCADE -- Se a categoria for excluída, os produtos dela também serão
);

-- ---------------------------------------------------------------------------------------------------
-- usuarios table
-- ---------------------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    senha_hash VARCHAR(255) NOT NULL
);

-- ---------------------------------------------------------------------------------------------------
-- restaurantes_produtos table (many-to-many) (ATUALIZADA com ON DELETE CASCADE)
-- ---------------------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS restaurantes_produtos (
    restaurante_id INT NOT NULL,
    produto_id INT NOT NULL,
    PRIMARY KEY (restaurante_id, produto_id),
    FOREIGN KEY (restaurante_id)
        REFERENCES restaurantes(id)
        ON DELETE CASCADE, -- Se o restaurante for excluído, os registros dessa tabela serão excluídos
    FOREIGN KEY (produto_id)
        REFERENCES produtos(id)
        ON DELETE CASCADE -- Boa prática: se o produto for excluído, a relação também deve ser
);