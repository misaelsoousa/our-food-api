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

-- ---------------------------------------------------------------------------------------------------
-- pedidos table (histórico de pedidos dos usuários)
-- ---------------------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS pedidos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario_id INT NOT NULL,
    restaurante_id INT NOT NULL,
    status ENUM('pendente', 'confirmado', 'preparando', 'entregue', 'cancelado') DEFAULT 'pendente',
    data_pedido DATETIME DEFAULT CURRENT_TIMESTAMP,
    data_entrega DATETIME NULL,
    valor_total DECIMAL(10,2) NOT NULL,
    taxa_entrega DECIMAL(10,2) DEFAULT 0.00,
    valor_final DECIMAL(10,2) NOT NULL,
    endereco_entrega VARCHAR(500) NOT NULL,
    observacoes TEXT NULL,
    metodo_pagamento ENUM('dinheiro', 'cartao_credito', 'cartao_debito', 'pix') DEFAULT 'dinheiro',
    avaliacao TINYINT NULL CHECK (avaliacao >= 1 AND avaliacao <= 5),
    comentario_avaliacao TEXT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (restaurante_id) REFERENCES restaurantes(id) ON DELETE CASCADE,
    INDEX idx_usuario_data (usuario_id, data_pedido),
    INDEX idx_restaurante_status (restaurante_id, status),
    INDEX idx_status_data (status, data_pedido)
);

-- ---------------------------------------------------------------------------------------------------
-- pedido_itens table (produtos dentro de cada pedido)
-- ---------------------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS pedido_itens (
    id INT AUTO_INCREMENT PRIMARY KEY,
    pedido_id INT NOT NULL,
    produto_id INT NOT NULL,
    quantidade INT NOT NULL DEFAULT 1,
    preco_unitario DECIMAL(10,2) NOT NULL,
    preco_total DECIMAL(10,2) NOT NULL,
    observacoes_item TEXT NULL,
    FOREIGN KEY (pedido_id) REFERENCES pedidos(id) ON DELETE CASCADE,
    FOREIGN KEY (produto_id) REFERENCES produtos(id) ON DELETE CASCADE,
    INDEX idx_pedido (pedido_id)
);

-- ---------------------------------------------------------------------------------------------------
-- produto_favoritos table (favoritos por usuário)
-- ---------------------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS produto_favoritos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario_id INT NOT NULL,
    produto_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY unique_usuario_produto (usuario_id, produto_id),
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    FOREIGN KEY (produto_id) REFERENCES produtos(id) ON DELETE CASCADE,
    INDEX idx_usuario (usuario_id),
    INDEX idx_produto (produto_id)
);
