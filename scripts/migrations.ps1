# Script para aplicar migrations no banco de dados

Write-Host "Aplicando migrations no banco de dados..." -ForegroundColor Green

docker-compose exec api dotnet ef database update --project /src/src/ConsultaCreditos.Infrastructure --startup-project /src/src/ConsultaCreditos.API

if ($LASTEXITCODE -eq 0) {
    Write-Host "Migrations aplicadas com sucesso!" -ForegroundColor Green
} else {
    Write-Host "Erro ao aplicar migrations!" -ForegroundColor Red
    Write-Host "Certifique-se de que os containers estão rodando (execute ./scripts/start.ps1 primeiro)" -ForegroundColor Yellow
    exit 1
}
