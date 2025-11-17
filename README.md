# 🛠️ Mão na Massa - API REST

Plataforma para preparar trabalhadores para o futuro do trabalho, oferecendo aprendizado acessível, certificação e conexão com oportunidades reais de renda.

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Decisões Arquiteturais](#decisões-arquiteturais)
- [Como Rodar](#como-rodar)
- [Variáveis de Ambiente](#variáveis-de-ambiente)
- [Rotas e Endpoints](#rotas-e-endpoints)
- [Exemplos de Uso](#exemplos-de-uso)
- [Estrutura do Projeto](#estrutura-do-projeto)

---

## 🎯 Visão Geral

O **Mão na Massa** é uma plataforma que ajuda pessoas que trabalham com ofícios manuais (pedreiros, pintores, eletricistas, encanadores, etc.) a:

- ✅ Aprender novas habilidades através de cursos práticos
- ✅ Obter certificados digitais validando conhecimento
- ✅ Criar perfis profissionais
- ✅ Oferecer serviços e ser encontrado por clientes
- ✅ Receber avaliações e construir reputação

### Tecnologias

- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 8.0**
- **SQLite** (banco de dados)
- **FluentValidation**
- **Swagger/OpenAPI**

---

## 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**:

```
MaoNaMassa.NET/
├── MaoNaMassa.Domain/          # Camada de Domínio
│   ├── Entities/               # Entidades de negócio
│   ├── Interfaces/             # Contratos de repositórios
│   └── Exceptions/             # Exceções de domínio
│
├── MaoNaMassa.Application/     # Camada de Aplicação
│   ├── DTOs/                   # Data Transfer Objects
│   │   ├── Input/              # DTOs de entrada
│   │   ├── Output/             # DTOs de saída
│   │   └── Paginacao/          # DTOs de paginação
│   ├── UseCases/               # Casos de uso
│   └── Validators/             # Validadores FluentValidation
│
├── MaoNaMassa.Infrastructure/  # Camada de Infraestrutura
│   ├── Data/                   # DbContext e Configurações EF
│   └── Repositories/           # Implementações dos repositórios
│
└── MaoNaMassa.API/             # Camada de Apresentação
    ├── Controllers/            # Controllers REST
    ├── Middleware/             # Middlewares customizados
    ├── Helpers/                # Helpers (HATEOAS)
    └── Extensions/             # Extensões de serviços
```

### Fluxo de Dados

```
Controller → Use Case → Repository → Database
     ↓           ↓           ↓
   DTOs      Regras      EF Core
   HATEOAS   Validação   Entities
```

---

## 🎨 Decisões Arquiteturais

### 1. **Clean Architecture**
- **Separação de responsabilidades**: Cada camada tem uma responsabilidade clara
- **Independência de frameworks**: O domínio não depende de frameworks externos
- **Testabilidade**: Fácil de testar cada camada isoladamente

### 2. **Domain-Driven Design (DDD)**
- **Rich Domain Model**: Entidades com comportamento e invariantes
- **Value Objects**: Validações encapsuladas nas entidades
- **Domain Exceptions**: Exceções específicas do domínio

### 3. **Repository Pattern**
- **Abstração de acesso a dados**: Interfaces no domínio, implementações na infraestrutura
- **Facilita testes**: Pode usar mocks ou in-memory database

### 4. **Use Cases**
- **Casos de uso claros**: Cada funcionalidade tem um caso de uso específico
- **Orquestração**: Use cases orquestram a lógica de negócio

### 5. **DTOs (Data Transfer Objects)**
- **Separação de camadas**: DTOs separam a camada de apresentação do domínio
- **Validação**: FluentValidation valida DTOs antes de processar

### 6. **HATEOAS (Hypermedia as the Engine of Application State)**
- **Navegação**: Links HATEOAS facilitam navegação entre recursos
- **Descoberta**: Clientes podem descobrir endpoints disponíveis

### 7. **In-Memory Database para Testes**
- **Desenvolvimento rápido**: Não precisa configurar banco de dados
- **Testes isolados**: Cada execução começa com banco limpo

---

## 🚀 Como Rodar

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 ou VS Code (opcional)

### Passo a Passo

1. **Clone o repositório**
   ```bash
   git clone <url-do-repositorio>
   cd MaoNaMassa.NET
   ```

2. **Restaurar dependências**
   ```bash
   dotnet restore
   ```

3. **Compilar o projeto**
   ```bash
   dotnet build
   ```

4. **Executar a API**
   ```bash
   cd MaoNaMassa.API
   dotnet run
   ```

5. **Acessar a API**
   - **Swagger UI**: http://localhost:5136/swagger
   - **Health Check**: http://localhost:5136/api/health
   - **API Base**: http://localhost:5136/api

### Migrations e Seed

O projeto usa **SQLite** como banco de dados. Para configurar:

1. **Instalar ferramenta EF Core CLI**
   ```bash
   dotnet tool install --global dotnet-ef
   ```

2. **Criar migration**
   ```bash
   dotnet ef migrations add InitialCreate --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
   ```

3. **Aplicar migration (criar banco)**
   ```bash
   dotnet ef database update --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
   ```

4. **Executar API**
   ```bash
   cd MaoNaMassa.API
   dotnet run
   ```

**Ou use o script automatizado:**
```powershell
.\setup-database.ps1
```

O banco de dados será criado como arquivo `MaoNaMassaDb.db` na pasta `MaoNaMassa.API/`.

**Para mais detalhes, consulte:** [SETUP_DATABASE.md](./SETUP_DATABASE.md)

---

### Migrations e Seed

O projeto usa **SQLite** como banco de dados. O arquivo `MaoNaMassaDb.db` será criado automaticamente na pasta `MaoNaMassa.API/` ao executar as migrations.

1. **Instalar ferramenta EF Core**
   ```bash
   dotnet tool install --global dotnet-ef
   ```

2. **Criar migration**
   ```bash
   dotnet ef migrations add InitialCreate --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
   ```

3. **Aplicar migration (cria o banco)**
   ```bash
   dotnet ef database update --project MaoNaMassa.Infrastructure --startup-project MaoNaMassa.API
   ```

**Ou use o script automatizado:**
```powershell
.\setup-database.ps1
```

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

Para desenvolvimento, você pode criar um arquivo `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=MaoNaMassaDb_Dev.db"
  }
}
```

### Variáveis de Ambiente (Opcional)

Você pode usar variáveis de ambiente para sobrescrever configurações:

```bash
# Windows PowerShell
$env:ASPNETCORE_ENVIRONMENT="Development"
$env:ConnectionStrings__DefaultConnection="Server=localhost;Database=MaoNaMassaDb;..."

# Linux/Mac
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection="Server=localhost;Database=MaoNaMassaDb;..."
```

---

## 🛣️ Rotas e Endpoints

### Base URL
```
http://localhost:5136/api
```

### Endpoints Disponíveis

#### 🔵 Usuários (`/api/usuarios`)

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
- `tipoUsuario` (string): Filtro por tipo
- `areaInteresse` (string): Filtro por área
- `ordenarPor` (string): Campo para ordenação (nome, datacriacao, cidade)
- `ordenarDescendente` (bool): Ordenação descendente

#### 🟢 Cursos (`/api/cursos`)

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
- `nivel` (string): Filtro por nível
- `ordenarPor` (string): titulo, datacriacao, area, nivel

#### 🟡 Aulas (`/api/aulas`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/aulas/curso/{cursoId}` | Lista aulas de um curso |
| GET | `/api/aulas/{id}` | Busca aula por ID |
| POST | `/api/aulas` | Cria nova aula |
| PUT | `/api/aulas/{id}` | Atualiza aula |
| DELETE | `/api/aulas/{id}` | Remove aula |

#### 🟠 Quizzes (`/api/quizzes`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/quizzes/curso/{cursoId}` | Lista quizzes de um curso |
| GET | `/api/quizzes/{id}` | Busca quiz por ID |
| POST | `/api/quizzes` | Cria novo quiz |
| PUT | `/api/quizzes/{id}` | Atualiza quiz |
| DELETE | `/api/quizzes/{id}` | Remove quiz |

#### 🔴 Serviços (`/api/servicos`)

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
- `ordenarPor` (string): titulo, datapublicacao, preco, avaliacao

#### 🟣 Profissionais (`/api/profissionais`)

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
- `apenasDisponiveis` (bool): Apenas disponíveis

#### ⚪ Avaliações (`/api/avaliacoes`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/avaliacoes/servico/{servicoId}` | Lista avaliações de um serviço |
| GET | `/api/avaliacoes/{id}` | Busca avaliação por ID |
| POST | `/api/avaliacoes?usuarioId={id}` | Cria nova avaliação |

#### 🟤 Certificados (`/api/certificados`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/certificados/usuario/{usuarioId}` | Lista certificados de um usuário |
| GET | `/api/certificados/{id}` | Busca certificado por ID |
| GET | `/api/certificados/codigo/{codigo}` | Busca por código |
| POST | `/api/certificados/completar-curso?usuarioId={id}` | Completa curso e gera certificado |

#### ⚫ Health Check

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/health` | Verifica saúde da API |

---

## 📝 Exemplos de Uso

### 1. Swagger UI (Recomendado)

Acesse http://localhost:5136/swagger para:
- Ver todos os endpoints
- Testar diretamente na interface
- Ver schemas dos DTOs
- Testar requisições e ver respostas

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

#### Listar Usuários (Paginação)
```bash
curl "http://localhost:5136/api/usuarios?pagina=1&tamanhoPagina=10"
```

#### Buscar Usuários (Filtros)
```bash
curl "http://localhost:5136/api/usuarios/search?nome=João&cidade=São Paulo&pagina=1&ordenarPor=nome"
```

#### Criar Curso
```bash
curl -X POST "http://localhost:5136/api/cursos" \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Instalações Elétricas Residenciais",
    "descricao": "Aprenda a fazer instalações elétricas seguras",
    "area": "Elétrica",
    "nivel": "Iniciante"
  }'
```

#### Buscar Serviços (Busca Avançada)
```bash
curl "http://localhost:5136/api/servicos/search?cidade=São Paulo&precoMaximo=500&avaliacaoMinima=4&ordenarPor=avaliacao&pagina=1"
```

#### Criar Serviço
```bash
curl -X POST "http://localhost:5136/api/servicos?profissionalId={GUID}" \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Instalação de Chuveiro Elétrico",
    "descricao": "Instalação completa com segurança",
    "cidade": "São Paulo",
    "preco": 250.00
  }'
```

#### Completar Curso (Gerar Certificado)
```bash
curl -X POST "http://localhost:5136/api/certificados/completar-curso?usuarioId={GUID}" \
  -H "Content-Type: application/json" \
  -d '{
    "cursoId": "{GUID}"
  }'
```

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

### 4. JavaScript (Fetch)

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
.then(data => console.log(data));

// Buscar serviços
fetch('http://localhost:5136/api/servicos/search?cidade=São Paulo&pagina=1')
  .then(response => response.json())
  .then(data => console.log(data));
```

### 5. Resposta com HATEOAS

Todas as respostas incluem links HATEOAS:

```json
{
  "data": {
    "id": "guid",
    "nome": "João Silva",
    ...
  },
  "links": [
    {
      "rel": "self",
      "href": "http://localhost:5136/api/usuarios/{id}",
      "method": "GET"
    },
    {
      "rel": "update",
      "href": "http://localhost:5136/api/usuarios/{id}",
      "method": "PUT"
    },
    {
      "rel": "delete",
      "href": "http://localhost:5136/api/usuarios/{id}",
      "method": "DELETE"
    }
  ]
}
```

### 6. Resposta Paginada

```json
{
  "data": {
    "itens": [...],
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

---

## 📁 Estrutura do Projeto

```
MaoNaMassa.NET/
│
├── MaoNaMassa.Domain/              # Domínio
│   ├── Entities/                    # 9 entidades
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
│   └── Exceptions/                 # Exceções de domínio
│
├── MaoNaMassa.Application/          # Aplicação
│   ├── DTOs/
│   │   ├── Input/                   # 11 DTOs de entrada
│   │   ├── Output/                  # 10 DTOs de saída
│   │   └── Paginacao/               # DTOs de paginação
│   ├── UseCases/                    # 8 casos de uso
│   └── Validators/                  # Validadores FluentValidation
│
├── MaoNaMassa.Infrastructure/      # Infraestrutura
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations/          # 9 configurações EF
│   └── Repositories/                # 9 implementações
│
└── MaoNaMassa.API/                 # API
    ├── Controllers/                 # 9 controllers
    ├── Middleware/                  # Tratamento de erros
    ├── Helpers/                     # HATEOAS helper
    └── Extensions/                  # Extensões de serviços
```

---

## 🎯 Funcionalidades Implementadas

✅ **CRUD Completo** para todas as entidades  
✅ **Busca Paginada** com ordenação e filtros  
✅ **HATEOAS** em todas as respostas  
✅ **Validação** com FluentValidation  
✅ **Tratamento de Erros** global com ProblemDetails  
✅ **Swagger/OpenAPI** para documentação  
✅ **Clean Architecture** e DDD  
✅ **Repository Pattern**  
✅ **Use Cases** para lógica de negócio  

---

## 📚 Documentação Adicional

- [Como Executar](./COMO_EXECUTAR.md)
- [Teste do Projeto](./TESTE_PROJETO.md)

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

