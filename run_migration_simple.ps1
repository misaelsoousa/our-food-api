# Script PowerShell simplificado para executar a migração
Write-Host "Executando migração do banco de dados..." -ForegroundColor Green

# Configurações do banco (ajuste conforme necessário)
$server = "localhost"
$database = "ourfood"  # Ajuste o nome do seu banco
$username = "root"      # Ajuste o usuário
$password = ""         # Ajuste a senha

try {
    # Comando SQL para executar
    $sqlCommand = @"
ALTER TABLE produtos 
ADD COLUMN restaurante_id INT NOT NULL DEFAULT 3;

ALTER TABLE produtos 
ADD CONSTRAINT FK_produtos_restaurante 
FOREIGN KEY (restaurante_id) REFERENCES restaurantes(id);

CREATE INDEX IX_produtos_restaurante_id ON produtos(restaurante_id);
"@

    Write-Host "Executando comandos SQL..." -ForegroundColor Yellow
    
    # Executar cada comando separadamente
    $commands = $sqlCommand -split ";" | Where-Object { $_.Trim() -ne "" }
    
    foreach ($cmd in $commands) {
        if ($cmd.Trim() -ne "") {
            Write-Host "Executando: $($cmd.Trim())" -ForegroundColor Cyan
            $fullCommand = "mysql -h $server -u $username -p$password $database -e `"$($cmd.Trim())`""
            Invoke-Expression $fullCommand
        }
    }
    
    Write-Host "Migração executada com sucesso!" -ForegroundColor Green
    Write-Host "Agora você pode executar a API novamente." -ForegroundColor Cyan
}
catch {
    Write-Host "Erro ao executar migração: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Tente executar manualmente no MySQL Workbench ou phpMyAdmin" -ForegroundColor Yellow
}


