# setup-database.ps1
# Script para configurar banco de dados SQLite e aplicar migrations

Write-Host "=== Setup do Banco de Dados Mão na Massa ===" -ForegroundColor Cyan

# 1. Verificar se dotnet-ef está instalado
Write-Host "`n[1/5] Verificando dotnet-ef..." -ForegroundColor Yellow
$efInstalled = dotnet ef --version 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "Instalando dotnet-ef..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
} else {
    Write-Host "✓ dotnet-ef já instalado: $efInstalled" -ForegroundColor Green
}

# 2. Verificar Connection String
Write-Host "`n[2/5] Verificando configuração..." -ForegroundColor Yellow
$appsettingsPath = "MaoNaMassa.API\appsettings.json"
if (Test-Path $appsettingsPath) {
    $appsettings = Get-Content $appsettingsPath | ConvertFrom-Json
    if ($appsettings.ConnectionStrings.DefaultConnection) {
        Write-Host "✓ Connection String configurada" -ForegroundColor Green
        Write-Host "  $($appsettings.ConnectionStrings.DefaultConnection)" -ForegroundColor Gray
    } else {
        Write-Host "✗ Connection String não encontrada!" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "✗ Arquivo appsettings.json não encontrado!" -ForegroundColor Red
    exit 1
}

# 3. Verificar se Program.cs está configurado para SQLite
Write-Host "`n[2.5/5] Verificando Program.cs..." -ForegroundColor Yellow
$programPath = "MaoNaMassa.API\Program.cs"
if (Test-Path $programPath) {
    $programContent = Get-Content $programPath -Raw
    if ($programContent -match "UseSqlite") {
        Write-Host "✓ Program.cs configurado para SQLite" -ForegroundColor Green
    } elseif ($programContent -match "UseInMemoryDatabase") {
        Write-Host "⚠ Program.cs ainda usa In-Memory Database!" -ForegroundColor Yellow
        Write-Host "  Altere para UseSqlite antes de continuar" -ForegroundColor Yellow
        $continue = Read-Host "Deseja continuar mesmo assim? (s/n)"
        if ($continue -ne "s") {
            exit 1
        }
    } elseif ($programContent -match "UseSqlite") {
        Write-Host "✓ Program.cs configurado para SQLite" -ForegroundColor Green
    }
}

# 4. Criar Migration
Write-Host "`n[3/5] Criando migration..." -ForegroundColor Yellow
$migrationName = "InitialCreate"
$migrationsPath = "MaoNaMassa.Infrastructure\Migrations"
$migrationExists = Test-Path $migrationsPath

if ($migrationExists -and (Get-ChildItem $migrationsPath -Filter "*.cs" | Measure-Object).Count -gt 0) {
    Write-Host "Migrations já existem. Criando nova migration..." -ForegroundColor Yellow
    $migrationName = "Migration_$(Get-Date -Format 'yyyyMMddHHmmss')"
}

Write-Host "Executando: dotnet ef migrations add $migrationName..." -ForegroundColor Gray
dotnet ef migrations add $migrationName --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Migration criada com sucesso!" -ForegroundColor Green
} else {
    Write-Host "✗ Erro ao criar migration!" -ForegroundColor Red
    Write-Host "Verifique se o Program.cs está configurado corretamente" -ForegroundColor Yellow
    exit 1
}

# 5. Aplicar Migration
Write-Host "`n[4/5] Aplicando migration ao banco de dados..." -ForegroundColor Yellow
Write-Host "Isso pode levar alguns segundos..." -ForegroundColor Gray
dotnet ef database update --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Banco de dados criado/atualizado com sucesso!" -ForegroundColor Green
} else {
    Write-Host "✗ Erro ao aplicar migration!" -ForegroundColor Red
    Write-Host "Verifique:" -ForegroundColor Yellow
    Write-Host "  - Connection String está correta?" -ForegroundColor Yellow
    Write-Host "  - Você tem permissões de escrita no diretório?" -ForegroundColor Yellow
    Write-Host "  - O caminho do arquivo está acessível?" -ForegroundColor Yellow
    exit 1
}

# 6. Verificar banco de dados criado
Write-Host "`n[5/5] Verificando banco de dados criado..." -ForegroundColor Yellow
$connectionString = $appsettings.ConnectionStrings.DefaultConnection
if ($connectionString -match "Data Source=([^;]+)") {
    $dbPath = $matches[1].Trim()
    # Se for caminho relativo, assume que está na pasta da API
    if (-not [System.IO.Path]::IsPathRooted($dbPath)) {
        $dbPath = Join-Path "MaoNaMassa.API" $dbPath
    }
    
    if (Test-Path $dbPath) {
        $dbFile = Get-Item $dbPath
        Write-Host "✓ Banco de dados criado:" -ForegroundColor Green
        Write-Host "  Arquivo: $($dbFile.FullName)" -ForegroundColor Gray
        Write-Host "  Tamanho: $([math]::Round($dbFile.Length / 1KB, 2)) KB" -ForegroundColor Gray
        Write-Host "  Data: $($dbFile.LastWriteTime)" -ForegroundColor Gray
        Write-Host "`n  Use DB Browser for SQLite para visualizar as tabelas" -ForegroundColor Yellow
    } else {
        Write-Host "⚠ Arquivo do banco não encontrado em: $dbPath" -ForegroundColor Yellow
    }
} else {
    Write-Host "⚠ Não foi possível extrair caminho do banco da connection string" -ForegroundColor Yellow
}

Write-Host "`n=== Setup concluído! ===" -ForegroundColor Cyan
Write-Host "Execute 'dotnet run --project MaoNaMassa.API' para iniciar a API" -ForegroundColor Yellow
Write-Host "Ou use: .\run.ps1" -ForegroundColor Yellow

