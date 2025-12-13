# Script para parar a aplicacao

Write-Host "Parando containers..." -ForegroundColor Yellow

docker-compose down

if ($LASTEXITCODE -eq 0) {
    Write-Host "Containers parados com sucesso!" -ForegroundColor Green
} else {
    Write-Host "Erro ao parar containers!" -ForegroundColor Red
    exit 1
}
