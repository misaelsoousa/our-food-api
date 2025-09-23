-- Create database if not exists
CREATE DATABASE IF NOT EXISTS ourfooddb;
USE ourfooddb;

-- categorias table
CREATE TABLE IF NOT EXISTS categorias (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    cor_hex VARCHAR(7) NOT NULL,
    imagem VARCHAR(255)
);

-- restaurantes table
CREATE TABLE IF NOT EXISTS restaurantes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    imagem VARCHAR(255)
);

-- produtos table
CREATE TABLE IF NOT EXISTS produtos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    imagem VARCHAR(255),
    preco DECIMAL(10,2),
    categoria_id INT,
    FOREIGN KEY (categoria_id) REFERENCES categorias(id)
);

-- usuarios table
CREATE TABLE IF NOT EXISTS usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    senha_hash VARCHAR(255) NOT NULL
);

-- restaurantes_produtos table (many-to-many)
CREATE TABLE IF NOT EXISTS restaurantes_produtos (
    restaurante_id INT NOT NULL,
    produto_id INT NOT NULL,
    PRIMARY KEY (restaurante_id, produto_id),
    FOREIGN KEY (restaurante_id) REFERENCES restaurantes(id),
    FOREIGN KEY (produto_id) REFERENCES produtos(id)
);
