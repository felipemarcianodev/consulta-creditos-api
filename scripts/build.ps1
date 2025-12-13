# Script para build do Docker

Write-Host "Iniciando build da imagem Docker..." -ForegroundColor Green

docker build -t consulta-creditos-api:latest .

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build concluido com sucesso!" -ForegroundColor Green
} else {
    Write-Host "Erro no build!" -ForegroundColor Red
    exit 1
}
