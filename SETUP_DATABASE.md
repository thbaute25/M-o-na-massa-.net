# 🗄️ Setup do Banco de Dados - SQLite

Guia completo para configurar o banco de dados SQLite e executar migrations.

## 📋 Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQLite (já incluído no pacote NuGet, não precisa instalação separada)
- [DB Browser for SQLite](https://sqlitebrowser.org/) (opcional, para visualizar o banco)

---

## 🚀 Passo a Passo

### 1. Instalar Ferramenta EF Core CLI

```powershell
# Instalar globalmente
dotnet tool install --global dotnet-ef

# Verificar instalação
dotnet ef --version
```

**Se já estiver instalado, atualize:**
```powershell
dotnet tool update --global dotnet-ef
```

---

### 2. Configurar Connection String

Edite o arquivo `MaoNaMassa.API/appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=MaoNaMassaDb.db"
  }
}
```

**Opções de Connection String:**

#### SQLite (Arquivo no diretório da API)
```json
"DefaultConnection": "Data Source=MaoNaMassaDb.db"
```

#### SQLite (Caminho absoluto)
```json
"DefaultConnection": "Data Source=C:\\Dados\\MaoNaMassaDb.db"
```

#### SQLite (Modo somente leitura)
```json
"DefaultConnection": "Data Source=MaoNaMassaDb.db;Mode=ReadOnly"
```

#### SQLite (Com cache compartilhado)
```json
"DefaultConnection": "Data Source=MaoNaMassaDb.db;Cache=Shared"
```

---

### 3. Verificar Program.cs

O `MaoNaMassa.API/Program.cs` já está configurado para usar SQLite:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString, sqlOptions => 
        sqlOptions.MigrationsAssembly("MaoNaMassa.Infrastructure"));
    
    // Apenas para desenvolvimento
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});
```

**Importante:** O pacote `Microsoft.EntityFrameworkCore.Sqlite` já está instalado no projeto Infrastructure.

---

### 4. Criar Migration

Execute no diretório raiz do projeto:

```powershell
# Navegar para o diretório raiz
cd "C:\Users\THenriquebaute\OneDrive - Alvarez and Marsal\Desktop\MaoNaMassa.NET"

# Criar migration inicial
dotnet ef migrations add InitialCreate --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
```

**O que acontece:**
- Cria uma pasta `Migrations/` em `MaoNaMassa.Infrastructure/`
- Gera arquivos de migration com timestamp
- Analisa todas as entidades e configurações do EF Core

---

### 5. Aplicar Migration (Criar Banco de Dados)

```powershell
# Aplicar migrations e criar o banco
dotnet ef database update --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
```

**O que acontece:**
- Cria o banco de dados `MaoNaMassaDb` (se não existir)
- Cria todas as tabelas baseadas nas entidades
- Aplica índices, chaves estrangeiras e constraints

---

### 6. Verificar Banco de Dados

#### Opção 1: DB Browser for SQLite (Recomendado)
1. Baixe e instale [DB Browser for SQLite](https://sqlitebrowser.org/)
2. Abra o DB Browser
3. Clique em "Open Database"
4. Navegue até `MaoNaMassa.API/MaoNaMassaDb.db`
5. Veja todas as tabelas e dados

#### Opção 2: Visual Studio Code
1. Instale a extensão "SQLite Viewer" ou "SQLite"
2. Abra o arquivo `MaoNaMassaDb.db` na pasta `MaoNaMassa.API/`
3. Visualize as tabelas e dados

#### Opção 3: PowerShell (Verificar se arquivo existe)
```powershell
Test-Path "MaoNaMassa.API\MaoNaMassaDb.db"
Get-Item "MaoNaMassa.API\MaoNaMassaDb.db" | Select-Object Name, Length, LastWriteTime
```

#### Opção 4: Linha de Comando SQLite
```powershell
# Se tiver sqlite3 instalado
sqlite3 MaoNaMassa.API\MaoNaMassaDb.db ".tables"
```

---

### 7. Executar a Aplicação

```powershell
cd MaoNaMassa.API
dotnet run
```

A API estará disponível em:
- **Swagger**: http://localhost:5136/swagger
- **Health**: http://localhost:5136/api/health

---

## 🔄 Comandos Úteis

### Criar Nova Migration
```powershell
dotnet ef migrations add NomeDaMigration --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
```

### Aplicar Migrations Pendentes
```powershell
dotnet ef database update --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
```

### Reverter Última Migration
```powershell
dotnet ef database update NomeDaMigrationAnterior --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
```

### Remover Última Migration (sem aplicar)
```powershell
dotnet ef migrations remove --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
```

### Listar Migrations
```powershell
dotnet ef migrations list --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
```

### Gerar Script SQL (sem aplicar)
```powershell
dotnet ef migrations script --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API --output migration.sql
```

### Remover Banco de Dados (CUIDADO!)
```powershell
dotnet ef database drop --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API --force
```

---

## 📝 Scripts PowerShell Automatizados

### Script Completo (setup-database.ps1)

Crie um arquivo `setup-database.ps1` na raiz do projeto:

```powershell
# setup-database.ps1
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
$appsettings = Get-Content "MaoNaMassa.API\appsettings.json" | ConvertFrom-Json
if ($appsettings.ConnectionStrings.DefaultConnection) {
    Write-Host "✓ Connection String configurada" -ForegroundColor Green
    Write-Host "  $($appsettings.ConnectionStrings.DefaultConnection)" -ForegroundColor Gray
} else {
    Write-Host "✗ Connection String não encontrada!" -ForegroundColor Red
    exit 1
}

# 3. Criar Migration
Write-Host "`n[3/5] Criando migration..." -ForegroundColor Yellow
$migrationName = "InitialCreate"
$migrationExists = Test-Path "MaoNaMassa.Infrastructure\Migrations"

if ($migrationExists) {
    Write-Host "Migrations já existem. Criando nova migration: $(Get-Date -Format 'yyyyMMddHHmmss')" -ForegroundColor Yellow
    $migrationName = "Migration_$(Get-Date -Format 'yyyyMMddHHmmss')"
}

dotnet ef migrations add $migrationName --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Migration criada com sucesso!" -ForegroundColor Green
} else {
    Write-Host "✗ Erro ao criar migration!" -ForegroundColor Red
    exit 1
}

# 4. Aplicar Migration
Write-Host "`n[4/5] Aplicando migration ao banco de dados..." -ForegroundColor Yellow
dotnet ef database update --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Banco de dados criado/atualizado com sucesso!" -ForegroundColor Green
} else {
    Write-Host "✗ Erro ao aplicar migration!" -ForegroundColor Red
    exit 1
}

# 5. Verificar banco de dados criado
Write-Host "`n[5/5] Verificando banco de dados..." -ForegroundColor Yellow
$dbPath = "MaoNaMassa.API\MaoNaMassaDb.db"
if (Test-Path $dbPath) {
    $dbInfo = Get-Item $dbPath
    Write-Host "✓ Banco de dados criado:" -ForegroundColor Green
    Write-Host "  Localização: $($dbInfo.FullName)" -ForegroundColor Gray
    Write-Host "  Tamanho: $([math]::Round($dbInfo.Length / 1KB, 2)) KB" -ForegroundColor Gray
    Write-Host "  Data: $($dbInfo.LastWriteTime)" -ForegroundColor Gray
} else {
    Write-Host "⚠ Arquivo do banco não encontrado (pode ser criado na primeira execução)" -ForegroundColor Yellow
}

Write-Host "`n=== Setup concluído! ===" -ForegroundColor Cyan
Write-Host "Execute 'dotnet run --project MaoNaMassa.API' para iniciar a API" -ForegroundColor Yellow
```

**Como usar:**
```powershell
.\setup-database.ps1
```

---

### Script de Execução Rápida (run.ps1)

Crie um arquivo `run.ps1`:

```powershell
# run.ps1 - Executa a API
Write-Host "=== Iniciando API Mão na Massa ===" -ForegroundColor Cyan

# Verificar se o banco existe
Write-Host "Verificando banco de dados..." -ForegroundColor Yellow
$dbPath = "MaoNaMassa.API\MaoNaMassaDb.db"
if (-not (Test-Path $dbPath)) {
    Write-Host "⚠ Banco de dados não encontrado!" -ForegroundColor Yellow
    Write-Host "Execute primeiro: .\setup-database.ps1" -ForegroundColor Yellow
    $continue = Read-Host "Deseja continuar mesmo assim? (s/n)"
    if ($continue -ne "s") {
        exit
    }
} else {
    Write-Host "✓ Banco de dados encontrado" -ForegroundColor Green
}

Write-Host "`nIniciando API..." -ForegroundColor Green
Write-Host "Swagger: http://localhost:5136/swagger" -ForegroundColor Cyan
Write-Host "Pressione Ctrl+C para parar`n" -ForegroundColor Yellow

cd MaoNaMassa.API
dotnet run
```

**Como usar:**
```powershell
.\run.ps1
```

---

## 🐛 Troubleshooting

### Erro: "dotnet-ef não é reconhecido"
```powershell
# Instalar novamente
dotnet tool install --global dotnet-ef
# Fechar e reabrir o terminal
```

### Erro: "Cannot open database"
- Verifique se o diretório existe e tem permissões de escrita
- Verifique a Connection String (caminho do arquivo)
- O arquivo será criado automaticamente na primeira migration

### Erro: "Migration já existe"
```powershell
# Remover última migration
dotnet ef migrations remove --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
# Criar novamente
dotnet ef migrations add InitialCreate --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
```

### Erro: "Database is locked"
- Feche outras conexões com o banco (DB Browser, outras instâncias da API)
- SQLite permite apenas uma escrita por vez

### Limpar tudo e começar do zero
```powershell
# Remover banco
dotnet ef database drop --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API --force

# Remover migrations
Remove-Item -Recurse -Force "MaoNaMassa.Infrastructure\Migrations"

# Criar novamente
dotnet ef migrations add InitialCreate --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
dotnet ef database update --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
```

---

## 📊 Estrutura do Banco de Dados

Após aplicar as migrations, você terá as seguintes tabelas:

- `Usuarios`
- `Cursos`
- `Aulas`
- `Quizzes`
- `RespostasQuiz`
- `Certificados`
- `Profissionais`
- `Servicos`
- `Avaliacoes`

Todas com relacionamentos, índices e constraints configurados.

---

## ✅ Checklist Final

- [ ] dotnet-ef instalado
- [ ] Connection String configurada
- [ ] Program.cs configurado para SQLite
- [ ] Migration criada
- [ ] Migration aplicada (banco criado)
- [ ] Tabelas verificadas
- [ ] API executando com sucesso

---

**Pronto! Seu banco de dados está configurado e pronto para uso! 🎉**

