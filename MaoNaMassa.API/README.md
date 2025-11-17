# MaoNaMassa.API

Camada de apresentação (API) do sistema Mão na Massa.

## 🛠️ Tratamento de Erros

### Middleware Global de Exceções

O sistema utiliza um middleware global (`GlobalExceptionHandlerMiddleware`) que captura todas as exceções não tratadas e retorna respostas no formato **ProblemDetails** (RFC 7807).

### Tipos de Erros Tratados

1. **EntityNotFoundException** → HTTP 404 (Not Found)
   - Quando uma entidade não é encontrada
   - Exemplo: "Usuário com ID {id} não foi encontrado."

2. **DomainException** → HTTP 400 (Bad Request)
   - Erros de validação de regras de negócio
   - Exemplo: "Usuário já possui certificado para este curso."

3. **ArgumentException** → HTTP 400 (Bad Request)
   - Argumentos inválidos passados para métodos

4. **UnauthorizedAccessException** → HTTP 401 (Unauthorized)
   - Acesso não autorizado

5. **Exception (genérica)** → HTTP 500 (Internal Server Error)
   - Erros inesperados do sistema

### Formato de Resposta (ProblemDetails)

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Erro de Validação de Domínio",
  "status": 400,
  "detail": "Mensagem de erro específica",
  "instance": "/api/usuarios/123",
  "errors": {
    "Nome": ["Nome é obrigatório."],
    "Email": ["Email inválido."]
  }
}
```

## ✅ Validações

### FluentValidation

O sistema utiliza **FluentValidation** para validação de DTOs de entrada.

#### Validators Implementados

- `CriarUsuarioRequestValidator` - Valida criação de usuário
- `CriarCursoRequestValidator` - Valida criação de curso
- `ResponderQuizRequestValidator` - Valida resposta de quiz
- `AvaliarServicoRequestValidator` - Valida avaliação de serviço
- `CriarServicoRequestValidator` - Valida criação de serviço
- `CriarPerfilProfissionalRequestValidator` - Valida criação de perfil profissional

#### Exemplo de Validação

```csharp
public class CriarUsuarioRequestValidator : AbstractValidator<CriarUsuarioRequest>
{
    public CriarUsuarioRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres.");
    }
}
```

### Validação Automática

As validações são executadas automaticamente antes de chegar aos controllers, retornando erros no formato ProblemDetails quando inválidas.

## 📋 Estrutura

```
MaoNaMassa.API/
├── Middleware/
│   └── GlobalExceptionHandlerMiddleware.cs  # Tratamento global de exceções
├── Extensions/
│   └── ServiceCollectionExtensions.cs       # Configuração de serviços
├── Program.cs                               # Configuração da aplicação
└── README.md                                # Esta documentação
```

## 🔧 Configuração

### Program.cs

```csharp
// Validações e tratamento de erros
builder.Services.AddValidationAndErrorHandling();

// Middleware de tratamento de erros (deve ser um dos primeiros)
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
```

## 📝 Exemplos de Uso

### Resposta de Erro de Validação

**Request:**
```json
POST /api/usuarios
{
  "nome": "Jo",
  "email": "email-invalido"
}
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Erro de Validação",
  "status": 400,
  "detail": "Um ou mais erros de validação ocorreram.",
  "instance": "/api/usuarios",
  "errors": {
    "Nome": ["Nome deve ter no mínimo 3 caracteres."],
    "Email": ["Email inválido."]
  }
}
```

### Resposta de Entidade Não Encontrada

**Request:**
```
GET /api/usuarios/00000000-0000-0000-0000-000000000000
```

**Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Recurso Não Encontrado",
  "status": 404,
  "detail": "Usuário com ID 00000000-0000-0000-0000-000000000000 não foi encontrado.",
  "instance": "/api/usuarios/00000000-0000-0000-0000-000000000000"
}
```

### Resposta de Erro de Domínio

**Request:**
```
POST /api/cursos/{cursoId}/completar
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Erro de Validação de Domínio",
  "status": 400,
  "detail": "Nota final 65.00% insuficiente para certificação. Mínimo exigido: 70%.",
  "instance": "/api/cursos/123/completar"
}
```

