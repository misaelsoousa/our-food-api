# Script para migrar imagens locais para S3
# Execute este script após configurar as credenciais AWS no appsettings.json

Write-Host "=== Migração de Imagens para S3 ===" -ForegroundColor Green
Write-Host "Este script irá migrar todas as imagens locais para o S3" -ForegroundColor Yellow
Write-Host "Certifique-se de que as credenciais AWS estão configuradas no appsettings.json" -ForegroundColor Yellow
Write-Host ""

# Verificar se o arquivo appsettings.json existe
if (-not (Test-Path "OurFood.Api/appsettings.json")) {
    Write-Host "ERRO: Arquivo appsettings.json não encontrado!" -ForegroundColor Red
    exit 1
}

# Verificar se as credenciais AWS estão configuradas
$appsettings = Get-Content "OurFood.Api/appsettings.json" | ConvertFrom-Json
if ($appsettings.AWS.AccessKeyId -eq "" -or $appsettings.AWS.SecretAccessKey -eq "") {
    Write-Host "ERRO: Credenciais AWS não configuradas no appsettings.json!" -ForegroundColor Red
    Write-Host "Configure as credenciais AWS antes de executar este script." -ForegroundColor Yellow
    exit 1
}

Write-Host "Credenciais AWS encontradas. Iniciando migração..." -ForegroundColor Green

# Compilar o projeto
Write-Host "Compilando projeto..." -ForegroundColor Cyan
dotnet build OurFood.Api/OurFood.Api.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERRO: Falha na compilação!" -ForegroundColor Red
    exit 1
}

Write-Host "Compilação bem-sucedida!" -ForegroundColor Green

# Executar migração
Write-Host "Executando migração de imagens..." -ForegroundColor Cyan
dotnet run --project OurFood.Api/OurFood.Api.csproj --migrate-images

Write-Host "Migração concluída!" -ForegroundColor Green
Write-Host "Todas as imagens foram migradas para o S3." -ForegroundColor Green
Write-Host "As URLs antigas agora funcionarão independentemente do IP da EC2." -ForegroundColor Green