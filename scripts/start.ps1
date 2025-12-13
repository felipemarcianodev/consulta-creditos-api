# Script para iniciar a aplicacao com Docker Compose

Write-Host "Verificando arquivo .env..." -ForegroundColor Yellow

if (-not (Test-Path ".env")) {
    Write-Host "Arquivo .env nao encontrado. Criando a partir do .env.example..." -ForegroundColor Yellow
    if (Test-Path ".env.example") {
        Copy-Item ".env.example" ".env"
        Write-Host "ATENCAO: Configure o arquivo .env com suas credenciais do Azure Service Bus!" -ForegroundColor Red
        Write-Host "Pressione qualquer tecla para continuar apos configurar o .env..." -ForegroundColor Yellow
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    } else {
        Write-Host "Arquivo .env.example nao encontrado!" -ForegroundColor Red
        exit 1
    }
}

Write-Host "Iniciando containers com Docker Compose..." -ForegroundColor Green

docker-compose up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "Containers iniciados com sucesso!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Acesse a API em: http://localhost:8080" -ForegroundColor Cyan
    Write-Host "Health check: http://localhost:8080/health/self" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Para ver os logs, execute: docker-compose logs -f" -ForegroundColor Yellow
} else {
    Write-Host "Erro ao iniciar containers!" -ForegroundColor Red
    exit 1
}
