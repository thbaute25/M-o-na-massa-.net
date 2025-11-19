# 🛠️ Mão na Massa - Plataforma Completa

Plataforma para preparar trabalhadores para o futuro do trabalho, oferecendo aprendizado acessível, certificação e conexão com oportunidades reais de renda.

---

## 📋 Índice

- [Visão Geral](#-visão-geral)
- [Decisões Arquiteturais](#-decisões-arquiteturais)
- [Como Rodar](#-como-rodar)
  - [Pré-requisitos](#pré-requisitos)
  - [Instalação](#instalação)
  - [Migrations e Seed](#migrations-e-seed)
- [Variáveis de Ambiente](#-variáveis-de-ambiente)
- [Rotas e Endpoints](#-rotas-e-endpoints)
  - [API REST](#api-rest)
  - [Razor Pages (Web UI)](#razor-pages-web-ui)
- [Exemplos de Uso](#-exemplos-de-uso)
  - [Swagger UI](#1-swagger-ui-recomendado)
  - [cURL](#2-curl)
  - [PowerShell](#3-powershell)
  - [JavaScript/Fetch](#4-javascriptfetch)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Funcionalidades Implementadas](#-funcionalidades-implementadas)

---

## 🎯 Visão Geral

O **Mão na Massa** é uma plataforma completa que ajuda pessoas que trabalham com ofícios manuais (pedreiros, pintores, eletricistas, encanadores, etc.) a:

- ✅ **Aprender novas habilidades** através de cursos práticos com aulas e quizzes
- ✅ **Obter certificados digitais** validando conhecimento após aprovação
- ✅ **Criar perfis profissionais** destacando habilidades e experiência
- ✅ **Oferecer serviços** e ser encontrado por clientes
- ✅ **Receber avaliações** e construir reputação profissional

### 🚀 Tecnologias Utilizadas

- **.NET 8.0** - Framework principal
- **ASP.NET Core Web API** - API REST
- **ASP.NET Core Razor Pages** - Interface web
- **Entity Framework Core 8.0** - ORM
- **SQLite** - Banco de dados
- **FluentValidation** - Validação de dados
- **Swagger/OpenAPI** - Documentação interativa
- **Bootstrap 5.3** - Framework CSS
- **jQuery Validation** - Validação client-side

### 🎨 Arquitetura

O projeto segue os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**:

```
┌─────────────────────────────────────────────────────────┐
│                  MaoNaMassa.API                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  │
│  │ Controllers │  │ Razor Pages  │  │  Middleware  │  │
│  │   (REST)    │  │   (Web UI)   │  │   (Errors)    │  │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘  │
└─────────┼──────────────────┼──────────────────┼─────────┘
          │                  │                  │
          └──────────────────┼──────────────────┘
                             │
┌────────────────────────────┼────────────────────────────┐
│                  MaoNaMassa.Application                │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐ │
│  │  Use Cases  │  │     DTOs     │  │  Validators  │ │
│  │  (Orquest.) │  │  (Input/Out) │  │ FluentValid. │ │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘ │
└─────────┼──────────────────┼──────────────────┼────────┘
           │                  │                  │
           └──────────────────┼──────────────────┘
                              │
┌─────────────────────────────┼─────────────────────────────┐
│                  MaoNaMassa.Domain                        │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐    │
│  │  Entities    │  │  Interfaces  │  │  Exceptions  │    │
│  │  (Rich Model)│  │ (Repository)  │  │   (Domain)   │    │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘    │
└─────────┼──────────────────┼──────────────────┼────────────┘
          │                  │                  │
          └──────────────────┼──────────────────┘
                              │
┌─────────────────────────────┼─────────────────────────────┐
│              MaoNaMassa.Infrastructure                   │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐    │
│  │   DbContext  │  │ Repositories │  │ Configurations│    │
│  │   (EF Core)  │  │  (Concrete)  │  │   (Mappings)  │    │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘    │
└─────────┼──────────────────┼──────────────────┼────────────┘
          │                  │                  │
          └──────────────────┼──────────────────┘
                              │
                    ┌─────────┴─────────┐
                    │   SQLite Database │
                    │  (MaoNaMassaDb.db)│
                    └───────────────────┘
```

---

## 🏗️ Decisões Arquiteturais

### 1. **Clean Architecture**

**Por quê?**
- Separação clara de responsabilidades entre camadas
- Independência de frameworks externos no domínio
- Facilita testes unitários e de integração
- Permite trocar tecnologias sem impactar o domínio

**Como?**
- **Domain**: Entidades puras, sem dependências externas
- **Application**: Casos de uso e regras de negócio
- **Infrastructure**: Implementações técnicas (EF Core, repositórios)
- **API**: Controllers e apresentação

### 2. **Domain-Driven Design (DDD)**

**Por quê?**
- Modelo rico que reflete o negócio
- Invariantes garantidas nas entidades
- Linguagem ubíqua entre desenvolvedores e stakeholders

**Como?**
- **Rich Domain Model**: Entidades com comportamento e validações
- **Invariantes**: Regras de negócio encapsuladas (ex: nota entre 1-5)
- **Factory Methods**: `Certificado.Criar()`, `Avaliacao.Criar()`
- **Domain Exceptions**: `DomainException`, `EntityNotFoundException`

### 3. **Repository Pattern**

**Por quê?**
- Abstração de acesso a dados
- Facilita testes com mocks
- Permite trocar implementação de persistência

**Como?**
- Interfaces no **Domain** (`IUsuarioRepository`)
- Implementações na **Infrastructure** (`UsuarioRepository`)
- Injeção de dependência no **Program.cs**

### 4. **Use Cases (Casos de Uso)**

**Por quê?**
- Casos de uso claros e específicos
- Orquestração de regras de negócio complexas
- Reutilização entre diferentes pontos de entrada

**Como?**
- `AvaliarServicoUseCase` - Orquestra avaliação completa
- `CompletarCursoUseCase` - Gera certificado após aprovação
- `BuscarServicosUseCase` - Busca com múltiplos filtros

### 5. **DTOs (Data Transfer Objects)**

**Por quê?**
- Separação entre camadas de apresentação e domínio
- Controle sobre o que é exposto na API
- Validação antes de processar

**Como?**
- **Input DTOs**: `CriarUsuarioRequest`, `AvaliarServicoRequest`
- **Output DTOs**: `UsuarioResponse`, `ServicoResponse`
- **Pagination DTOs**: `PaginacaoRequest`, `PaginacaoResponse<T>`

### 6. **HATEOAS (Hypermedia as the Engine of Application State)**

**Por quê?**
- Navegação facilitada entre recursos
- Descoberta de endpoints disponíveis
- APIs mais RESTful e exploráveis

**Como?**
- `HateoasHelper` cria links automaticamente
- Links de paginação (first, prev, next, last)
- Links de recursos (self, update, delete)

### 7. **Dual Interface (API REST + Razor Pages)**

**Por quê?**
- API REST para integração com outros sistemas
- Razor Pages para interface web amigável
- Mesma lógica de negócio compartilhada

**Como?**
- **API Controllers**: `/api/*` endpoints REST
- **Razor Pages**: `/`, `/Home`, `/Cursos`, etc.
- Ambos usam os mesmos Use Cases e Repositories

### 8. **FluentValidation**

**Por quê?**
- Validação declarativa e legível
- Separação de validação da lógica de negócio
- Mensagens de erro customizadas

**Como?**
- Validators em `MaoNaMassa.Application/Validators/`
- Validação automática via `AddValidationAndErrorHandling()`
- Retorna `ProblemDetails` (RFC 7807) em caso de erro

### 9. **SQLite**

**Por quê?**
- Simplicidade: arquivo único, sem servidor
- Ideal para desenvolvimento e testes
- Fácil backup e portabilidade

**Como?**
- Banco criado automaticamente na pasta `MaoNaMassa.API/`
- Migrations via EF Core
- Seed automático de dados iniciais

---

## 🚀 Como Rodar

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
- Visual Studio 2022, VS Code ou Rider (opcional)
- PowerShell (Windows) ou Terminal (Linux/Mac)

### Instalação

1. **Clone o repositório**
   ```bash
   git clone <url-do-repositorio>
   cd MaoNaMassa.NET
   ```

2. **Restaurar dependências NuGet**
   ```bash
   dotnet restore
   ```

3. **Compilar o projeto**
   ```bash
   dotnet build
   ```

4. **Instalar ferramenta EF Core CLI** (se ainda não tiver)
   ```bash
   dotnet tool install --global dotnet-ef
   ```

### Migrations e Seed

#### Opção 1: Script Automatizado (Recomendado)

Execute o script PowerShell na raiz do projeto:

```powershell
.\setup-database.ps1
```

O script irá:
- ✅ Verificar se `dotnet-ef` está instalado
- ✅ Verificar configuração do `appsettings.json`
- ✅ Criar migration (se necessário)
- ✅ Aplicar migration (criar banco de dados)
- ✅ Verificar se o banco foi criado

#### Opção 2: Manual

1. **Criar migration**
   ```bash
   dotnet ef migrations add InitialCreate --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
   ```

2. **Aplicar migration (criar banco)**
   ```bash
   dotnet ef database update --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
   ```

3. **Verificar banco criado**
   ```bash
   # Windows PowerShell
   Test-Path "MaoNaMassa.API\MaoNaMassaDb.db"
   
   # Linux/Mac
   ls MaoNaMassa.API/MaoNaMassaDb.db
   ```

### Executar a Aplicação

```bash
cd MaoNaMassa.API
dotnet run
```

**Ou use o script:**
```powershell
.\run.ps1
```

### Acessar a Aplicação

Após executar, acesse:

- 🌐 **Interface Web (Razor Pages)**: http://localhost:5136/
- 📚 **Swagger UI (API REST)**: http://localhost:5136/swagger
- ❤️ **Health Check**: http://localhost:5136/api/health
- 🔌 **API Base**: http://localhost:5136/api

### Seed Automático

O projeto possui **seed automático** que popula o banco com dados iniciais:

- ✅ **8 Cursos** de exemplo (Elétrica, Pintura, Encanamento, etc.)
- ✅ **10 Usuários** de exemplo (Aprendizes, Profissionais, Clientes, Empresa)

O seed é executado automaticamente na primeira execução (ver `Program.cs` linha 68-82).

---

## 🔧 Variáveis de Ambiente

### appsettings.json

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

### appsettings.Development.json

Crie este arquivo para configurações de desenvolvimento:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=MaoNaMassaDb_Dev.db"
  }
}
```

### Variáveis de Ambiente (Opcional)

Você pode sobrescrever configurações via variáveis de ambiente:

#### Windows PowerShell
```powershell
$env:ASPNETCORE_ENVIRONMENT="Development"
$env:ConnectionStrings__DefaultConnection="Data Source=C:\Dados\MaoNaMassaDb.db"
```

#### Linux/Mac
```bash
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection="Data Source=/home/user/MaoNaMassaDb.db"
```

#### Docker (futuro)
```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Production
  - ConnectionStrings__DefaultConnection=Data Source=/app/data/MaoNaMassaDb.db
```

### Configurações Disponíveis

| Variável | Descrição | Padrão |
|----------|-----------|--------|
| `ASPNETCORE_ENVIRONMENT` | Ambiente (Development/Production) | `Production` |
| `ConnectionStrings__DefaultConnection` | String de conexão SQLite | `Data Source=MaoNaMassaDb.db` |
| `Logging__LogLevel__Default` | Nível de log padrão | `Information` |

---

## 🛣️ Rotas e Endpoints

### Base URLs

- **API REST**: `http://localhost:5136/api`
- **Web UI**: `http://localhost:5136`
- **Swagger**: `http://localhost:5136/swagger`

---

## API REST

### 🔵 Usuários (`/api/usuarios`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/usuarios` | Lista usuários (paginação) |
| GET | `/api/usuarios/{id}` | Busca usuário por ID |
| GET | `/api/usuarios/search` | Busca com filtros e paginação |
| POST | `/api/usuarios` | Cria novo usuário |
| PUT | `/api/usuarios/{id}` | Atualiza usuário |
| DELETE | `/api/usuarios/{id}` | Remove usuário |

**Query Parameters (GET /search):**
- `pagina` (int): Número da página (padrão: 1)
- `tamanhoPagina` (int): Itens por página (padrão: 10)
- `nome` (string): Filtro por nome
- `cidade` (string): Filtro por cidade
- `tipoUsuario` (string): Filtro por tipo (Aprendiz, Profissional, Cliente, Empresa)
- `areaInteresse` (string): Filtro por área
- `ordenarPor` (string): Campo para ordenação (`nome`, `datacriacao`, `cidade`)
- `ordenarDescendente` (bool): Ordenação descendente

### 🟢 Cursos (`/api/cursos`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/cursos` | Lista cursos (paginação) |
| GET | `/api/cursos/{id}` | Busca curso por ID |
| GET | `/api/cursos/search` | Busca com filtros |
| POST | `/api/cursos` | Cria novo curso |
| PUT | `/api/cursos/{id}` | Atualiza curso |
| DELETE | `/api/cursos/{id}` | Remove curso |

**Query Parameters (GET /search):**
- `titulo` (string): Busca por título ou descrição
- `area` (string): Filtro por área
- `nivel` (string): Filtro por nível (Iniciante, Intermediário, Avançado)
- `ordenarPor` (string): `titulo`, `datacriacao`, `area`, `nivel`

### 🟡 Aulas (`/api/aulas`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/aulas/curso/{cursoId}` | Lista aulas de um curso |
| GET | `/api/aulas/{id}` | Busca aula por ID |
| POST | `/api/aulas` | Cria nova aula |
| PUT | `/api/aulas/{id}` | Atualiza aula |
| DELETE | `/api/aulas/{id}` | Remove aula |

### 🟠 Quizzes (`/api/quizzes`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/quizzes/curso/{cursoId}` | Lista quizzes de um curso |
| GET | `/api/quizzes/{id}` | Busca quiz por ID |
| POST | `/api/quizzes` | Cria novo quiz |
| PUT | `/api/quizzes/{id}` | Atualiza quiz |
| DELETE | `/api/quizzes/{id}` | Remove quiz |

### 🔴 Serviços (`/api/servicos`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/servicos` | Lista serviços (paginação) |
| GET | `/api/servicos/{id}` | Busca serviço por ID |
| GET | `/api/servicos/search` | **Busca avançada com filtros** |
| POST | `/api/servicos?profissionalId={id}` | Cria novo serviço |
| PUT | `/api/servicos/{id}` | Atualiza serviço |
| DELETE | `/api/servicos/{id}` | Remove serviço |

**Query Parameters (GET /search):**
- `cidade` (string): Filtro por cidade
- `termo` (string): Busca em título/descrição
- `precoMaximo` (decimal): Preço máximo
- `avaliacaoMinima` (decimal): Avaliação mínima (0-5)
- `ordenarPor` (string): `titulo`, `datapublicacao`, `preco`, `avaliacao`

### 🟣 Profissionais (`/api/profissionais`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/profissionais` | Lista profissionais (paginação) |
| GET | `/api/profissionais/{id}` | Busca profissional por ID |
| GET | `/api/profissionais/search` | **Busca com filtros** |
| POST | `/api/profissionais?usuarioId={id}` | Cria perfil profissional |

**Query Parameters (GET /search):**
- `cidade` (string): Filtro por cidade
- `areaInteresse` (string): Filtro por área
- `avaliacaoMinima` (decimal): Avaliação mínima
- `apenasDisponiveis` (bool): Apenas disponíveis (padrão: true)

### ⚪ Avaliações (`/api/avaliacoes`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/avaliacoes/servico/{servicoId}` | Lista avaliações de um serviço |
| GET | `/api/avaliacoes/{id}` | Busca avaliação por ID |
| POST | `/api/avaliacoes` | Cria nova avaliação |

### 🟤 Certificados (`/api/certificados`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/certificados/usuario/{usuarioId}` | Lista certificados de um usuário |
| GET | `/api/certificados/{id}` | Busca certificado por ID |
| GET | `/api/certificados/codigo/{codigo}` | Busca por código |
| POST | `/api/certificados/completar-curso?usuarioId={id}` | Completa curso e gera certificado |

### ⚫ Health Check

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/health` | Verifica saúde da API |

---

## Razor Pages (Web UI)

### Rotas Padrão

| Rota | Página | Descrição |
|------|--------|-----------|
| `/` | Index.cshtml | Redireciona para `/Home` |
| `/Home` | Home.cshtml | Página inicial |
| `/Cursos` | Cursos.cshtml | Lista de cursos |
| `/Servicos` | Servicos.cshtml | Lista de serviços |
| `/Profissionais` | Profissionais.cshtml | Lista de profissionais |
| `/buscar` | Buscar.cshtml | Busca personalizada |

### Rotas Personalizadas

| Rota | Página | Descrição |
|------|--------|-----------|
| `/curso/{id:guid}` | Curso/Detalhes.cshtml | Detalhes do curso |
| `/curso/criar` | Curso/Criar.cshtml | Criar novo curso |
| `/servico/{id:guid}` | Servico/Detalhes.cshtml | Detalhes do serviço |
| `/servico/criar` | Servico/Criar.cshtml | Criar novo serviço |
| `/profissional/{id:guid}` | Profissional/Detalhes.cshtml | Detalhes do profissional |
| `/profissional/criar` | Profissional/Criar.cshtml | Criar perfil profissional |
| `/usuario/criar` | Usuario/Criar.cshtml | Criar novo usuário |
| `/usuario/{id:guid}/perfil` | Usuario/Perfil.cshtml | Perfil do usuário |
| `/usuario/{id:guid}/certificados` | Usuario/Certificados.cshtml | Certificados do usuário |
| `/avaliacao/criar` | Avaliacao/Criar.cshtml | Avaliar serviço |
| `/quiz/responder` | Quiz/Responder.cshtml | Responder quiz |
| `/certificado/{codigo}` | Certificado/Verificar.cshtml | Verificar certificado |
| `/area/{area}/cursos` | Area/Cursos.cshtml | Cursos por área |
| `/cidade/{cidade}/servicos` | Cidade/Servicos.cshtml | Serviços por cidade |

---

## 📝 Exemplos de Uso

### 1. Swagger UI (Recomendado)

Acesse **http://localhost:5136/swagger** para:

- ✅ Ver todos os endpoints disponíveis
- ✅ Testar diretamente na interface
- ✅ Ver schemas dos DTOs
- ✅ Testar requisições e ver respostas
- ✅ Ver exemplos de requisições e respostas

**Como usar:**
1. Abra http://localhost:5136/swagger
2. Expanda o endpoint desejado
3. Clique em "Try it out"
4. Preencha os parâmetros
5. Clique em "Execute"
6. Veja a resposta abaixo

---

### 2. cURL

#### Criar Usuário

```bash
curl -X POST "http://localhost:5136/api/usuarios" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "email": "joao@email.com",
    "senha": "senha123",
    "cidade": "São Paulo",
    "areaInteresse": "Elétrica",
    "tipoUsuario": "Profissional"
  }'
```

**Resposta:**
```json
{
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "nome": "João Silva",
    "email": "joao@email.com",
    "cidade": "São Paulo",
    "areaInteresse": "Elétrica",
    "tipoUsuario": "Profissional",
    "dataCriacao": "2024-01-15T10:30:00Z",
    "temPerfilProfissional": false,
    "totalCertificados": 0
  },
  "links": [
    {
      "rel": "self",
      "href": "http://localhost:5136/api/usuarios/123e4567-e89b-12d3-a456-426614174000",
      "method": "GET"
    },
    {
      "rel": "update",
      "href": "http://localhost:5136/api/usuarios/123e4567-e89b-12d3-a456-426614174000",
      "method": "PUT"
    },
    {
      "rel": "delete",
      "href": "http://localhost:5136/api/usuarios/123e4567-e89b-12d3-a456-426614174000",
      "method": "DELETE"
    }
  ]
}
```

#### Listar Usuários (Paginação)

```bash
curl "http://localhost:5136/api/usuarios?pagina=1&tamanhoPagina=10"
```

#### Buscar Usuários (Filtros)

```bash
curl "http://localhost:5136/api/usuarios/search?nome=João&cidade=São Paulo&pagina=1&ordenarPor=nome&ordenarDescendente=false"
```

#### Criar Curso

```bash
curl -X POST "http://localhost:5136/api/cursos" \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Instalações Elétricas Residenciais",
    "descricao": "Aprenda a fazer instalações elétricas seguras em residências",
    "area": "Elétrica",
    "nivel": "Iniciante"
  }'
```

#### Buscar Serviços (Busca Avançada)

```bash
curl "http://localhost:5136/api/servicos/search?cidade=São Paulo&precoMaximo=500&avaliacaoMinima=4&ordenarPor=avaliacao&pagina=1&tamanhoPagina=10"
```

#### Criar Serviço

```bash
curl -X POST "http://localhost:5136/api/servicos?profissionalId=123e4567-e89b-12d3-a456-426614174000" \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Instalação de Chuveiro Elétrico",
    "descricao": "Instalação completa com segurança e garantia",
    "cidade": "São Paulo",
    "preco": 250.00
  }'
```

#### Avaliar Serviço

```bash
curl -X POST "http://localhost:5136/api/avaliacoes" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": "123e4567-e89b-12d3-a456-426614174000",
    "servicoId": "223e4567-e89b-12d3-a456-426614174000",
    "nota": 5,
    "comentario": "Excelente serviço, muito profissional!"
  }'
```

#### Completar Curso (Gerar Certificado)

```bash
curl -X POST "http://localhost:5136/api/certificados/completar-curso?usuarioId=123e4567-e89b-12d3-a456-426614174000" \
  -H "Content-Type: application/json" \
  -d '{
    "cursoId": "323e4567-e89b-12d3-a456-426614174000"
  }'
```

---

### 3. PowerShell

#### Criar Usuário

```powershell
$body = @{
    nome = "João Silva"
    email = "joao@email.com"
    senha = "senha123"
    cidade = "São Paulo"
    areaInteresse = "Elétrica"
    tipoUsuario = "Profissional"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5136/api/usuarios" `
    -Method POST `
    -ContentType "application/json" `
    -Body $body
```

#### Listar Usuários

```powershell
Invoke-RestMethod -Uri "http://localhost:5136/api/usuarios?pagina=1&tamanhoPagina=10"
```

#### Buscar Serviços

```powershell
$params = @{
    cidade = "São Paulo"
    precoMaximo = 500
    avaliacaoMinima = 4
    ordenarPor = "avaliacao"
    pagina = 1
    tamanhoPagina = 10
}

$queryString = ($params.GetEnumerator() | ForEach-Object { "$($_.Key)=$($_.Value)" }) -join '&'
Invoke-RestMethod -Uri "http://localhost:5136/api/servicos/search?$queryString"
```

---

### 4. JavaScript/Fetch

#### Criar Usuário

```javascript
// Criar usuário
fetch('http://localhost:5136/api/usuarios', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    nome: 'João Silva',
    email: 'joao@email.com',
    senha: 'senha123',
    cidade: 'São Paulo',
    areaInteresse: 'Elétrica',
    tipoUsuario: 'Profissional'
  })
})
.then(response => response.json())
.then(data => {
  console.log('Usuário criado:', data.data);
  console.log('Links disponíveis:', data.links);
})
.catch(error => console.error('Erro:', error));
```

#### Buscar Serviços

```javascript
// Buscar serviços com filtros
const params = new URLSearchParams({
  cidade: 'São Paulo',
  precoMaximo: '500',
  avaliacaoMinima: '4',
  ordenarPor: 'avaliacao',
  pagina: '1',
  tamanhoPagina: '10'
});

fetch(`http://localhost:5136/api/servicos/search?${params}`)
  .then(response => response.json())
  .then(data => {
    console.log('Serviços encontrados:', data.data.itens);
    console.log('Total:', data.data.totalItens);
    console.log('Links de paginação:', data.links);
  })
  .catch(error => console.error('Erro:', error));
```

#### Usar Links HATEOAS

```javascript
// Seguir links HATEOAS
fetch('http://localhost:5136/api/usuarios/123e4567-e89b-12d3-a456-426614174000')
  .then(response => response.json())
  .then(data => {
    // Encontrar link de atualização
    const updateLink = data.links.find(link => link.rel === 'update');
    
    // Atualizar usando o link
    fetch(updateLink.href, {
      method: updateLink.method,
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        nome: 'João Silva Atualizado',
        cidade: 'Rio de Janeiro'
      })
    })
    .then(response => response.json())
    .then(updated => console.log('Atualizado:', updated));
  });
```

---

### 5. Resposta com HATEOAS

Todas as respostas incluem links HATEOAS:

```json
{
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "nome": "João Silva",
    "email": "joao@email.com",
    ...
  },
  "links": [
    {
      "rel": "self",
      "href": "http://localhost:5136/api/usuarios/123e4567-e89b-12d3-a456-426614174000",
      "method": "GET"
    },
    {
      "rel": "update",
      "href": "http://localhost:5136/api/usuarios/123e4567-e89b-12d3-a456-426614174000",
      "method": "PUT"
    },
    {
      "rel": "delete",
      "href": "http://localhost:5136/api/usuarios/123e4567-e89b-12d3-a456-426614174000",
      "method": "DELETE"
    }
  ]
}
```

### 6. Resposta Paginada

```json
{
  "data": {
    "itens": [
      {
        "id": "123e4567-e89b-12d3-a456-426614174000",
        "nome": "João Silva",
        ...
      },
      ...
    ],
    "paginaAtual": 1,
    "tamanhoPagina": 10,
    "totalItens": 50,
    "totalPaginas": 5,
    "temPaginaAnterior": false,
    "temProximaPagina": true
  },
  "links": [
    {
      "rel": "self",
      "href": "http://localhost:5136/api/usuarios?pagina=1",
      "method": "GET"
    },
    {
      "rel": "next",
      "href": "http://localhost:5136/api/usuarios?pagina=2",
      "method": "GET"
    },
    {
      "rel": "last",
      "href": "http://localhost:5136/api/usuarios?pagina=5",
      "method": "GET"
    }
  ]
}
```

### 7. Tratamento de Erros (ProblemDetails)

Em caso de erro, a API retorna `ProblemDetails` (RFC 7807):

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Erro de Validação de Domínio",
  "status": 400,
  "detail": "Usuário não pode avaliar seu próprio serviço.",
  "instance": "/api/avaliacoes",
  "errors": {
    "nota": [
      "A nota deve estar entre 1 e 5"
    ]
  }
}
```

---

## 📁 Estrutura do Projeto

```
MaoNaMassa.NET/
│
├── MaoNaMassa.Domain/              # Camada de Domínio
│   ├── Entities/                    # 9 entidades de negócio
│   │   ├── Usuario.cs
│   │   ├── Curso.cs
│   │   ├── Aula.cs
│   │   ├── Quiz.cs
│   │   ├── RespostaQuiz.cs
│   │   ├── Certificado.cs
│   │   ├── Profissional.cs
│   │   ├── Servico.cs
│   │   └── Avaliacao.cs
│   ├── Interfaces/                 # 9 interfaces de repositório
│   │   ├── IUsuarioRepository.cs
│   │   ├── ICursoRepository.cs
│   │   └── ...
│   └── Exceptions/                 # Exceções de domínio
│       ├── DomainException.cs
│       └── EntityNotFoundException.cs
│
├── MaoNaMassa.Application/          # Camada de Aplicação
│   ├── DTOs/
│   │   ├── Input/                   # DTOs de entrada
│   │   │   ├── CriarUsuarioRequest.cs
│   │   │   ├── AvaliarServicoRequest.cs
│   │   │   └── ...
│   │   ├── Output/                  # DTOs de saída
│   │   │   ├── UsuarioResponse.cs
│   │   │   ├── ServicoResponse.cs
│   │   │   └── ...
│   │   └── Paginacao/               # DTOs de paginação
│   │       ├── PaginacaoRequest.cs
│   │       └── PaginacaoResponse.cs
│   ├── UseCases/                    # 8 casos de uso
│   │   ├── AvaliarServicoUseCase.cs
│   │   ├── CompletarCursoUseCase.cs
│   │   ├── BuscarServicosUseCase.cs
│   │   └── ...
│   └── Validators/                  # Validadores FluentValidation
│       ├── CriarUsuarioRequestValidator.cs
│       └── ...
│
├── MaoNaMassa.Infrastructure/      # Camada de Infraestrutura
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   ├── DbInitializer.cs        # Seed de dados iniciais
│   │   └── Configurations/          # 9 configurações EF Core
│   │       ├── UsuarioConfiguration.cs
│   │       ├── CursoConfiguration.cs
│   │       └── ...
│   ├── Repositories/                # 9 implementações de repositório
│   │   ├── UsuarioRepository.cs
│   │   ├── CursoRepository.cs
│   │   └── ...
│   └── Migrations/                 # Migrations do EF Core
│       └── 20251117184154_InitialCreate.cs
│
└── MaoNaMassa.API/                 # Camada de Apresentação
    ├── Controllers/                 # 9 controllers REST
    │   ├── UsuariosController.cs
    │   ├── CursosController.cs
    │   ├── ServicosController.cs
    │   └── ...
    ├── Pages/                       # Razor Pages (Web UI)
    │   ├── Home.cshtml
    │   ├── Cursos.cshtml
    │   ├── Curso/
    │   │   ├── Criar.cshtml
    │   │   └── Detalhes.cshtml
    │   └── ...
    ├── Middleware/                  # Middlewares customizados
    │   └── GlobalExceptionHandlerMiddleware.cs
    ├── Helpers/                     # Helpers
    │   └── HateoasHelper.cs
    ├── Extensions/                  # Extensões de serviços
    │   └── ServiceCollectionExtensions.cs
    ├── ViewModels/                  # ViewModels para Razor Pages
    │   ├── CriarCursoViewModel.cs
    │   └── ...
    └── wwwroot/                     # Arquivos estáticos
        └── css/
            └── site.css
```

---

## ✅ Funcionalidades Implementadas

### Domínio & Arquitetura (20 pts)
- ✅ **9 Entidades** com invariantes e regras de negócio
- ✅ **Rich Domain Model** com comportamento encapsulado
- ✅ **Factory Methods** para criação de entidades
- ✅ **Domain Exceptions** para tratamento de erros

### Aplicação (20 pts)
- ✅ **8 Use Cases** claros e específicos
- ✅ **DTOs** para comunicação entre camadas
- ✅ **FluentValidation** para validação de entrada
- ✅ **ProblemDetails** (RFC 7807) para tratamento de erros

### Infra & Dados (20 pts)
- ✅ **EF Core** com mapeamentos completos
- ✅ **9 Repositórios** concretos com CRUD
- ✅ **Migrations** criadas e aplicadas
- ✅ **Seed automático** de dados iniciais

### Web (30 pts)
- ✅ **API REST** completa com HATEOAS
- ✅ **Razor Pages** com Bootstrap
- ✅ **Busca avançada** com paginação, ordenação e filtros
- ✅ **Validação** client-side e server-side

---

## 📚 Documentação Adicional

- [Setup do Banco de Dados](./SETUP_DATABASE.md) - Guia completo de migrations
- [Rotas Personalizadas](./ROTAS_PERSONALIZADAS.md) - Documentação de rotas
- [Rotas da Aplicação](./ROTAS_APLICACAO.md) - Todas as rotas disponíveis

---

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

---

## 📄 Licença

Este projeto é parte de um trabalho acadêmico.

---

**Desenvolvido com ❤️ para o futuro do trabalho**
