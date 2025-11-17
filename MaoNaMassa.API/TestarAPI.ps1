# Script para testar a API
Write-Host "=== Testando API Mão na Massa ===" -ForegroundColor Green
Write-Host ""

# Verificar se a API está rodando
Write-Host "Verificando se a API está respondendo..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5136/api/health" -Method Get -TimeoutSec 5
    Write-Host "✅ API está FUNCIONANDO!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Resposta:" -ForegroundColor Cyan
    $response | ConvertTo-Json
    Write-Host ""
    Write-Host "Acesse no navegador:" -ForegroundColor Yellow
    Write-Host "  - Swagger: http://localhost:5136/swagger" -ForegroundColor Cyan
    Write-Host "  - Health: http://localhost:5136/api/health" -ForegroundColor Cyan
} catch {
    Write-Host "❌ API NÃO está respondendo!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Possíveis causas:" -ForegroundColor Yellow
    Write-Host "  1. A API não está rodando" -ForegroundColor White
    Write-Host "     → Execute: cd MaoNaMassa.API; dotnet run" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  2. A API está rodando em outra porta" -ForegroundColor White
    Write-Host "     → Verifique a mensagem 'Now listening on:' no terminal" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  3. Firewall bloqueando" -ForegroundColor White
    Write-Host "     → Tente desabilitar temporariamente o firewall" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Erro detalhado: $_" -ForegroundColor Red
}

