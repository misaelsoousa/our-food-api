# Script para executar a migração SQL
$mysqlHost = "localhost"
$mysqlUser = "root"
$mysqlPassword = ""  # Deixe vazio se não tiver senha, ou coloque sua senha aqui
$mysqlDatabase = "ourfood"

Write-Host "Executando migração do banco de dados..."

# Comando SQL para adicionar a coluna restaurante_id
$sqlCommand = @"
ALTER TABLE produtos 
ADD COLUMN restaurante_id INT NOT NULL DEFAULT 3;

ALTER TABLE produtos 
ADD CONSTRAINT FK_produtos_restaurante_id 
FOREIGN KEY (restaurante_id) REFERENCES restaurantes(id);

CREATE INDEX IX_produtos_restaurante_id ON produtos(restaurante_id);
"@

try {
    # Executa o comando SQL
    $command = "mysql -h $mysqlHost -u $mysqlUser -p$mysqlPassword $mysqlDatabase -e `"$sqlCommand`""
    Invoke-Expression $command
    
    Write-Host "Migração executada com sucesso!"
    
    # Verifica se a coluna foi criada
    $verifyCommand = "mysql -h $mysqlHost -u $mysqlUser -p$mysqlPassword $mysqlDatabase -e `"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'produtos' AND COLUMN_NAME = 'restaurante_id';`""
    Invoke-Expression $verifyCommand
    
} catch {
    Write-Error "Erro ao executar migração: $($_.Exception.Message)"
    Write-Host "Verifique se:"
    Write-Host "1. O MySQL está rodando"
    Write-Host "2. O banco de dados 'ourfood' existe"
    Write-Host "3. As credenciais estão corretas"
}
