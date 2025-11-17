# run.ps1 - Executa a API
Write-Host "=== Iniciando API Mão na Massa ===" -ForegroundColor Cyan

# Verificar se o banco existe (opcional)
Write-Host "Verificando configuração..." -ForegroundColor Yellow
$appsettingsPath = "MaoNaMassa.API\appsettings.json"
if (Test-Path $appsettingsPath) {
    $appsettings = Get-Content $appsettingsPath | ConvertFrom-Json
    if ($appsettings.ConnectionStrings.DefaultConnection) {
        Write-Host "✓ Connection String configurada" -ForegroundColor Green
        $connectionString = $appsettings.ConnectionStrings.DefaultConnection
        if ($connectionString -match "Data Source=([^;]+)") {
            $dbPath = $matches[1].Trim()
            if (-not [System.IO.Path]::IsPathRooted($dbPath)) {
                $dbPath = Join-Path "MaoNaMassa.API" $dbPath
            }
            if (Test-Path $dbPath) {
                Write-Host "✓ Banco de dados encontrado: $dbPath" -ForegroundColor Green
            } else {
                Write-Host "⚠ Banco de dados será criado automaticamente na primeira execução" -ForegroundColor Yellow
            }
        }
    }
}

# Verificar se Program.cs está configurado
$programPath = "MaoNaMassa.API\Program.cs"
if (Test-Path $programPath) {
    $programContent = Get-Content $programPath -Raw
    if ($programContent -match "UseInMemoryDatabase") {
        Write-Host "⚠ Usando In-Memory Database (dados serão perdidos ao reiniciar)" -ForegroundColor Yellow
    } elseif ($programContent -match "UseSqlite") {
        Write-Host "✓ Usando SQLite" -ForegroundColor Green
        $dbPath = if ($appsettings.ConnectionStrings.DefaultConnection -match "Data Source=([^;]+)") { 
            $matches[1].Trim() 
        } else { 
            "MaoNaMassaDb.db" 
        }
        if (-not [System.IO.Path]::IsPathRooted($dbPath)) {
            $dbPath = Join-Path "MaoNaMassa.API" $dbPath
        }
        if (Test-Path $dbPath) {
            Write-Host "  Banco: $dbPath" -ForegroundColor Gray
        }
    } elseif ($programContent -match "UseSqlite") {
        Write-Host "✓ Usando SQLite" -ForegroundColor Green
    }
}

Write-Host "`nIniciando API..." -ForegroundColor Green
Write-Host "Swagger UI: http://localhost:5136/swagger" -ForegroundColor Cyan
Write-Host "Health Check: http://localhost:5136/api/health" -ForegroundColor Cyan
Write-Host "API Base: http://localhost:5136/api" -ForegroundColor Cyan
Write-Host "`nPressione Ctrl+C para parar`n" -ForegroundColor Yellow

Set-Location MaoNaMassa.API
dotnet run

