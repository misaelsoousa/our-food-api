# Script PowerShell para executar a migração do banco de dados
# Certifique-se de ter o MySQL instalado e configurado

Write-Host "Executando migração do banco de dados..." -ForegroundColor Green

# Configurações do banco (ajuste conforme necessário)
$server = "localhost"
$database = "ourfood"  # Ajuste o nome do seu banco
$username = "root"      # Ajuste o usuário
$password = ""         # Ajuste a senha

# Caminho para o arquivo SQL
$sqlFile = "execute_migration.sql"

try {
    # Executar o SQL usando mysql command line
    $command = "mysql -h $server -u $username -p$password $database < $sqlFile"
    
    Write-Host "Executando comando: $command" -ForegroundColor Yellow
    
    # Executar o comando
    Invoke-Expression $command
    
    Write-Host "Migração executada com sucesso!" -ForegroundColor Green
    Write-Host "Agora você pode executar a API novamente." -ForegroundColor Cyan
}
catch {
    Write-Host "Erro ao executar migração: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Verifique se:" -ForegroundColor Yellow
    Write-Host "1. O MySQL está instalado e rodando" -ForegroundColor Yellow
    Write-Host "2. As credenciais estão corretas" -ForegroundColor Yellow
    Write-Host "3. O banco de dados existe" -ForegroundColor Yellow
    Write-Host "4. Você tem permissões para alterar a tabela" -ForegroundColor Yellow
}



