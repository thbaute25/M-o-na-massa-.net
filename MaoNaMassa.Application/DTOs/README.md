# DTOs (Data Transfer Objects) e ViewModels

Este diretório contém todos os DTOs e ViewModels para comunicação entre camadas do sistema.

## 📁 Estrutura de Organização

```
DTOs/
├── Input/          # DTOs de entrada (Requests)
├── Output/         # DTOs de saída (Responses)
├── ViewModels/     # ViewModels para apresentação
└── Paginacao/      # DTOs para paginação
```

## 📥 Input DTOs (Requests)

DTOs usados para receber dados das requisições HTTP:

- `CriarUsuarioRequest` - Criar novo usuário
- `AtualizarUsuarioRequest` - Atualizar dados do usuário
- `CriarCursoRequest` - Criar novo curso
- `CriarAulaRequest` - Criar nova aula
- `CriarQuizRequest` - Criar novo quiz
- `ResponderQuizRequest` - Responder um quiz
- `CompletarCursoRequest` - Completar curso e gerar certificado
- `CriarPerfilProfissionalRequest` - Criar perfil profissional
- `CriarServicoRequest` - Criar novo serviço
- `AvaliarServicoRequest` - Avaliar um serviço
- `BuscarServicosRequest` - Buscar serviços com filtros
- `BuscarProfissionaisRequest` - Buscar profissionais com filtros

## 📤 Output DTOs (Responses)

DTOs usados para retornar dados nas respostas HTTP:

- `UsuarioResponse` - Informações do usuário
- `CursoResponse` - Informações do curso
- `CursoCompletoResponse` - Curso com aulas e quizzes
- `AulaResponse` - Informações da aula
- `QuizResponse` - Informações do quiz
- `ResultadoQuizResponse` - Resultado da resposta do quiz
- `CertificadoResponse` - Informações do certificado
- `ProfissionalResponse` - Informações do profissional
- `ServicoResponse` - Informações do serviço
- `AvaliacaoResponse` - Informações da avaliação

## 🎨 ViewModels

ViewModels para apresentação na interface do usuário:

- `UsuarioViewModel` - Dados formatados do usuário
- `CursoViewModel` - Dados formatados do curso
- `ServicoViewModel` - Dados formatados do serviço
- `ProfissionalViewModel` - Dados formatados do profissional
- `DashboardViewModel` - Dados do dashboard
- `CertificadoViewModel` - Certificado formatado

## 📄 Paginação

DTOs para requisições e respostas paginadas:

- `PaginacaoRequest` - Parâmetros de paginação
- `PaginacaoResponse<T>` - Resposta paginada genérica

## 🔄 Fluxo de Dados

```
Controller (HTTP Request)
    ↓
Input DTO (Request)
    ↓
Use Case / Service
    ↓
Output DTO (Response)
    ↓
ViewModel (opcional - para formatação)
    ↓
Controller (HTTP Response)
```

## 📝 Convenções

### Nomenclatura
- **Requests**: `[Acao]Request` (ex: `CriarUsuarioRequest`)
- **Responses**: `[Entidade]Response` (ex: `UsuarioResponse`)
- **ViewModels**: `[Entidade]ViewModel` (ex: `UsuarioViewModel`)

### Propriedades
- Requests: Apenas dados necessários para a operação
- Responses: Dados completos da entidade
- ViewModels: Dados formatados para apresentação (com propriedades de formatação)

## 🎯 Exemplos de Uso

### Request
```csharp
var request = new CriarUsuarioRequest
{
    Nome = "João Silva",
    Email = "joao@email.com",
    Senha = "senha123",
    Cidade = "São Paulo",
    AreaInteresse = "Elétrica",
    TipoUsuario = "Aprendiz"
};
```

### Response
```csharp
var response = new UsuarioResponse
{
    Id = Guid.NewGuid(),
    Nome = "João Silva",
    Email = "joao@email.com",
    // ... outros campos
};
```

### ViewModel
```csharp
var viewModel = new UsuarioViewModel
{
    Nome = "João Silva",
    TipoUsuarioDisplay = "Aprendiz",
    DataCriacaoFormatada = "15/01/2025",
    // ... outros campos formatados
};
```

