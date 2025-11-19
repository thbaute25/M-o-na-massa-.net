# 📸 Exemplos de Uso - Mão na Massa

Este documento contém exemplos práticos e visuais de como usar a API e a interface web.

---

## 🖥️ Interface Web (Razor Pages)

### Página Inicial

Acesse **http://localhost:5136/** para ver a página inicial com:

- ✅ Cards informativos sobre Cursos, Serviços e Profissionais
- ✅ Navegação Bootstrap responsiva
- ✅ Links rápidos para funcionalidades principais

### Criar Curso

1. Acesse **http://localhost:5136/curso/criar**
2. Preencha o formulário:
   - **Título**: Ex: "Instalação Elétrica Residencial"
   - **Descrição**: Descrição detalhada do curso
   - **Área**: Ex: "Elétrica", "Pintura", "Encanamento"
   - **Nível**: Iniciante, Intermediário ou Avançado
3. Clique em "Criar Curso"
4. Você será redirecionado para a página de detalhes do curso criado

**Validação:**
- Campos obrigatórios são validados
- Mensagens de erro aparecem abaixo dos campos
- Validação client-side (JavaScript) e server-side (C#)

### Listar Cursos

Acesse **http://localhost:5136/Cursos** para ver:

- ✅ Lista de todos os cursos disponíveis
- ✅ Cards com informações resumidas
- ✅ Links para detalhes de cada curso

### Buscar Serviços

Acesse **http://localhost:5136/buscar** para:

- ✅ Buscar por termo (título ou descrição)
- ✅ Filtrar por tipo (Curso, Serviço, Profissional)
- ✅ Filtrar por cidade
- ✅ Ver resultados paginados

---

## 🔌 API REST (Swagger UI)

### Acessar Swagger

1. Execute a aplicação: `dotnet run --project MaoNaMassa.API`
2. Acesse **http://localhost:5136/swagger**
3. Você verá todos os endpoints disponíveis organizados por controller

### Testar Endpoint no Swagger

#### Exemplo: Criar Usuário

1. Expanda o endpoint `POST /api/usuarios`
2. Clique em **"Try it out"**
3. Preencha o JSON:

```json
{
  "nome": "Maria Santos",
  "email": "maria@email.com",
  "senha": "senha123",
  "cidade": "Rio de Janeiro",
  "areaInteresse": "Pintura",
  "tipoUsuario": "Profissional"
}
```

4. Clique em **"Execute"**
5. Veja a resposta abaixo com:
   - Status Code: `201 Created`
   - Response Body com dados do usuário criado
   - Links HATEOAS

#### Exemplo: Buscar Serviços com Filtros

1. Expanda o endpoint `GET /api/servicos/search`
2. Clique em **"Try it out"**
3. Preencha os parâmetros:
   - `cidade`: São Paulo
   - `precoMaximo`: 500
   - `avaliacaoMinima`: 4
   - `ordenarPor`: avaliacao
   - `pagina`: 1
   - `tamanhoPagina`: 10
4. Clique em **"Execute"**
5. Veja a resposta paginada com links HATEOAS

---

## 📋 Exemplos de Requisições cURL

### 1. Health Check

```bash
curl http://localhost:5136/api/health
```

**Resposta:**
```json
{
  "status": "Healthy",
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### 2. Listar Usuários (Primeira Página)

```bash
curl "http://localhost:5136/api/usuarios?pagina=1&tamanhoPagina=5"
```

### 3. Buscar Usuários por Nome

```bash
curl "http://localhost:5136/api/usuarios/search?nome=João&pagina=1"
```

### 4. Criar Usuário

```bash
curl -X POST "http://localhost:5136/api/usuarios" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Pedro Oliveira",
    "email": "pedro@email.com",
    "senha": "senha123",
    "cidade": "Belo Horizonte",
    "areaInteresse": "Encanamento",
    "tipoUsuario": "Aprendiz"
  }'
```

### 5. Buscar Serviços em São Paulo

```bash
curl "http://localhost:5136/api/servicos/search?cidade=São Paulo&pagina=1&tamanhoPagina=10"
```

### 6. Criar Serviço

```bash
# Primeiro, obtenha um ID de profissional
PROFISSIONAL_ID="123e4567-e89b-12d3-a456-426614174000"

curl -X POST "http://localhost:5136/api/servicos?profissionalId=$PROFISSIONAL_ID" \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Instalação de Chuveiro Elétrico",
    "descricao": "Instalação completa com segurança e garantia",
    "cidade": "São Paulo",
    "preco": 250.00
  }'
```

### 7. Avaliar Serviço

```bash
curl -X POST "http://localhost:5136/api/avaliacoes" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": "123e4567-e89b-12d3-a456-426614174000",
    "servicoId": "223e4567-e89b-12d3-a456-426614174000",
    "nota": 5,
    "comentario": "Excelente trabalho, muito profissional!"
  }'
```

### 8. Buscar Profissionais Disponíveis

```bash
curl "http://localhost:5136/api/profissionais/search?apenasDisponiveis=true&cidade=São Paulo&pagina=1"
```

---

## 🔗 Exemplos de Links HATEOAS

### Resposta de um Recurso Individual

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

### Resposta Paginada

```json
{
  "data": {
    "itens": [...],
    "paginaAtual": 2,
    "tamanhoPagina": 10,
    "totalItens": 50,
    "totalPaginas": 5,
    "temPaginaAnterior": true,
    "temProximaPagina": true
  },
  "links": [
    {
      "rel": "self",
      "href": "http://localhost:5136/api/usuarios/search?pagina=2&nome=João",
      "method": "GET"
    },
    {
      "rel": "first",
      "href": "http://localhost:5136/api/usuarios/search?pagina=1&nome=João",
      "method": "GET"
    },
    {
      "rel": "prev",
      "href": "http://localhost:5136/api/usuarios/search?pagina=1&nome=João",
      "method": "GET"
    },
    {
      "rel": "next",
      "href": "http://localhost:5136/api/usuarios/search?pagina=3&nome=João",
      "method": "GET"
    },
    {
      "rel": "last",
      "href": "http://localhost:5136/api/usuarios/search?pagina=5&nome=João",
      "method": "GET"
    }
  ]
}
```

---

## 🧪 Testando Regras de Negócio

### Teste 1: Usuário não pode avaliar próprio serviço

```bash
# Criar serviço para um profissional
PROFISSIONAL_ID="123e4567-e89b-12d3-a456-426614174000"
curl -X POST "http://localhost:5136/api/servicos?profissionalId=$PROFISSIONAL_ID" \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Serviço Teste",
    "descricao": "Descrição teste",
    "cidade": "São Paulo",
    "preco": 100
  }'

# Tentar avaliar com o mesmo usuário do profissional
# Isso deve retornar erro 400
curl -X POST "http://localhost:5136/api/avaliacoes" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": "123e4567-e89b-12d3-a456-426614174000",
    "servicoId": "SERVICO_ID_AQUI",
    "nota": 5,
    "comentario": "Teste"
  }'
```

**Resposta esperada:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Erro de Validação de Domínio",
  "status": 400,
  "detail": "Usuário não pode avaliar seu próprio serviço.",
  "instance": "/api/avaliacoes"
}
```

### Teste 2: Avaliação duplicada

```bash
# Criar primeira avaliação
curl -X POST "http://localhost:5136/api/avaliacoes" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": "USUARIO_ID",
    "servicoId": "SERVICO_ID",
    "nota": 5
  }'

# Tentar criar segunda avaliação (deve falhar)
curl -X POST "http://localhost:5136/api/avaliacoes" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": "USUARIO_ID",
    "servicoId": "SERVICO_ID",
    "nota": 4
  }'
```

**Resposta esperada:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Erro de Validação de Domínio",
  "status": 400,
  "detail": "Usuário já avaliou este serviço.",
  "instance": "/api/avaliacoes"
}
```

### Teste 3: Certificado só é gerado com nota >= 70

```bash
# Responder quizzes incorretamente (nota < 70)
# Tentar gerar certificado
curl -X POST "http://localhost:5136/api/certificados/completar-curso?usuarioId=USUARIO_ID" \
  -H "Content-Type: application/json" \
  -d '{
    "cursoId": "CURSO_ID"
  }'
```

**Resposta esperada:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Erro de Validação de Domínio",
  "status": 400,
  "detail": "Nota final insuficiente para gerar certificado. Nota mínima: 70.",
  "instance": "/api/certificados/completar-curso"
}
```

---

## 📊 Fluxo Completo de Uso

### 1. Criar Usuário Profissional

```bash
curl -X POST "http://localhost:5136/api/usuarios" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Carlos Eletricista",
    "email": "carlos@email.com",
    "senha": "senha123",
    "cidade": "São Paulo",
    "areaInteresse": "Elétrica",
    "tipoUsuario": "Profissional"
  }'
```

**Salve o `id` retornado para próximos passos.**

### 2. Criar Perfil Profissional

```bash
PROFISSIONAL_ID="ID_DO_USUARIO_CRIADO"

curl -X POST "http://localhost:5136/api/profissionais?usuarioId=$PROFISSIONAL_ID" \
  -H "Content-Type: application/json" \
  -d '{
    "descricao": "Eletricista com 10 anos de experiência em instalações residenciais e comerciais."
  }'
```

### 3. Criar Serviço

```bash
curl -X POST "http://localhost:5136/api/servicos?profissionalId=$PROFISSIONAL_ID" \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Instalação de Chuveiro Elétrico",
    "descricao": "Instalação completa com segurança e garantia",
    "cidade": "São Paulo",
    "preco": 250.00
  }'
```

**Salve o `id` do serviço criado.**

### 4. Criar Usuário Cliente

```bash
curl -X POST "http://localhost:5136/api/usuarios" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Maria Cliente",
    "email": "maria@email.com",
    "senha": "senha123",
    "cidade": "São Paulo",
    "areaInteresse": "Reformas",
    "tipoUsuario": "Cliente"
  }'
```

**Salve o `id` do cliente.**

### 5. Avaliar Serviço

```bash
CLIENTE_ID="ID_DO_CLIENTE"
SERVICO_ID="ID_DO_SERVICO"

curl -X POST "http://localhost:5136/api/avaliacoes" \
  -H "Content-Type: application/json" \
  -d "{
    \"usuarioId\": \"$CLIENTE_ID\",
    \"servicoId\": \"$SERVICO_ID\",
    \"nota\": 5,
    \"comentario\": \"Excelente trabalho, muito profissional!\"
  }"
```

### 6. Verificar Avaliação Média Atualizada

```bash
curl "http://localhost:5136/api/servicos/$SERVICO_ID"
```

A resposta deve incluir `avaliacaoMedia: 5.0` e `totalAvaliacoes: 1`.

---

## 🎯 Dicas de Uso

### 1. Use Swagger para Explorar

O Swagger UI é a melhor forma de explorar a API:
- Veja todos os endpoints disponíveis
- Teste diretamente na interface
- Veja exemplos de requisições e respostas
- Copie comandos cURL gerados automaticamente

### 2. Siga os Links HATEOAS

Use os links retornados nas respostas para navegar:
- `self`: Link para o recurso atual
- `update`: Link para atualizar o recurso
- `delete`: Link para deletar o recurso
- `next`, `prev`, `first`, `last`: Links de paginação

### 3. Use Filtros e Ordenação

Aproveite os endpoints `/search` para:
- Filtrar resultados por múltiplos critérios
- Ordenar por diferentes campos
- Paginar resultados grandes

### 4. Validação de Erros

Todos os erros retornam `ProblemDetails` (RFC 7807):
- `status`: Código HTTP
- `title`: Título do erro
- `detail`: Mensagem detalhada
- `errors`: Erros de validação (quando aplicável)

---

**Para mais informações, consulte o [README.md](./README.md)**

