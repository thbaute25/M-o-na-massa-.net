# 🛣️ Rotas Personalizadas - Mão na Massa

## 📋 Rotas Personalizadas Implementadas

### 1. Rotas com Parâmetros GUID (Constraints)

| Rota | Página | Parâmetro | Constraint | Exemplo |
|------|--------|-----------|------------|---------|
| `/curso/{id:guid}` | Curso/Detalhes.cshtml | `id` | GUID | `/curso/123e4567-e89b-12d3-a456-426614174000` |
| `/servico/{id:guid}` | Servico/Detalhes.cshtml | `id` | GUID | `/servico/123e4567-e89b-12d3-a456-426614174000` |
| `/profissional/{id:guid}` | Profissional/Detalhes.cshtml | `id` | GUID | `/profissional/123e4567-e89b-12d3-a456-426614174000` |

### 2. Rotas com Parâmetros String (Required)

| Rota | Página | Parâmetro | Constraint | Exemplo |
|------|--------|-----------|------------|---------|
| `/certificado/{codigo:required}` | Certificado/Verificar.cshtml | `codigo` | Required | `/certificado/CERT-2024-001` |
| `/area/{area:required}/cursos` | Area/Cursos.cshtml | `area` | Required | `/area/Elétrica/cursos` |
| `/cidade/{cidade:required}/servicos` | Cidade/Servicos.cshtml | `cidade` | Required | `/cidade/São Paulo/servicos` |

### 3. Rotas com Query Parameters

| Rota | Página | Query Parameters | Exemplo |
|------|--------|------------------|---------|
| `/buscar` | Buscar.cshtml | `termo`, `tipo`, `cidade` | `/buscar?termo=elétrica&tipo=curso&cidade=São Paulo` |

### 4. Rotas Aninhadas (Hierárquicas)

| Rota | Página | Descrição |
|------|--------|-----------|
| `/usuario/{id:guid}/perfil` | Usuario/Perfil.cshtml | Perfil do usuário |
| `/usuario/{id:guid}/certificados` | Usuario/Certificados.cshtml | Certificados do usuário |

---

## 🎯 Tipos de Constraints Utilizados

### `{id:guid}`
- Valida que o parâmetro é um GUID válido
- Exemplo: `/curso/123e4567-e89b-12d3-a456-426614174000`
- Se não for GUID, retorna 404

### `{codigo:required}`
- Valida que o parâmetro é obrigatório e não vazio
- Exemplo: `/certificado/CERT-2024-001`
- Aceita qualquer string não vazia

### Query Parameters
- Parâmetros opcionais na URL
- Exemplo: `/buscar?termo=elétrica&tipo=curso`
- Acessados via `[BindProperty(SupportsGet = true)]`

---

## 📝 Exemplos de Uso

### Rotas com GUID
```html
<!-- Link para detalhes do curso -->
<a href="/curso/@cursoId">Ver Detalhes</a>

<!-- Link para detalhes do serviço -->
<a href="/servico/@servicoId">Ver Serviço</a>

<!-- Link para perfil do profissional -->
<a href="/profissional/@profissionalId">Ver Profissional</a>
```

### Rotas com String
```html
<!-- Verificar certificado -->
<a href="/certificado/@codigoCertificado">Verificar Certificado</a>

<!-- Cursos por área -->
<a href="/area/Elétrica/cursos">Cursos de Elétrica</a>

<!-- Serviços por cidade -->
<a href="/cidade/São Paulo/servicos">Serviços em São Paulo</a>
```

### Rotas com Query Parameters
```html
<!-- Busca simples -->
<a href="/buscar?termo=elétrica">Buscar "elétrica"</a>

<!-- Busca com filtros -->
<a href="/buscar?termo=pintura&tipo=servico&cidade=Rio de Janeiro">
    Buscar serviços de pintura no Rio
</a>
```

### Rotas Aninhadas
```html
<!-- Perfil do usuário -->
<a href="/usuario/@usuarioId/perfil">Meu Perfil</a>

<!-- Certificados do usuário -->
<a href="/usuario/@usuarioId/certificados">Meus Certificados</a>
```

---

## 🔧 Configuração no Program.cs

As rotas personalizadas são configuradas diretamente nas páginas Razor usando a diretiva `@page`:

```razor
@page "/curso/{id:guid}"  // Rota com constraint GUID
@page "/buscar"           // Rota simples
@page "/area/{area:required}/cursos"  // Rota com parâmetro obrigatório
```

O `Program.cs` apenas mapeia todas as Razor Pages:

```csharp
app.MapRazorPages(); // Mapeia todas as rotas definidas com @page
```

---

## 📊 Estrutura de Rotas

```
/                           → Index (redireciona para /Home)
/Home                       → Página inicial
/Cursos                     → Lista de cursos
/curso/{id:guid}            → Detalhes do curso
/area/{area}/cursos         → Cursos por área
/Servicos                   → Lista de serviços
/servico/{id:guid}          → Detalhes do serviço
/cidade/{cidade}/servicos   → Serviços por cidade
/Profissionais              → Lista de profissionais
/profissional/{id:guid}      → Detalhes do profissional
/buscar                     → Busca personalizada (com query params)
/usuario/{id:guid}/perfil   → Perfil do usuário
/usuario/{id:guid}/certificados → Certificados do usuário
/certificado/{codigo}       → Verificar certificado
/api/*                      → Endpoints da API REST
/swagger                    → Documentação da API
```

---

## ✅ Funcionalidades Implementadas

✅ **Rotas com constraints** (GUID, required)  
✅ **Rotas com parâmetros** (string, guid)  
✅ **Rotas com query parameters**  
✅ **Rotas aninhadas/hierárquicas**  
✅ **Validação de parâmetros** via constraints  
✅ **Breadcrumbs** para navegação  
✅ **Links entre páginas** funcionando  

---

## 🚀 Como Testar

1. **Iniciar a aplicação:**
   ```powershell
   dotnet run --project MaoNaMassa.API
   ```

2. **Testar rotas personalizadas:**
   - Detalhes do curso: http://localhost:5136/curso/{guid-do-curso}
   - Detalhes do serviço: http://localhost:5136/servico/{guid-do-servico}
   - Busca: http://localhost:5136/buscar?termo=elétrica&tipo=curso
   - Cursos por área: http://localhost:5136/area/Elétrica/cursos
   - Serviços por cidade: http://localhost:5136/cidade/São Paulo/servicos

**Nota:** Substitua `{guid-do-curso}` e `{guid-do-servico}` por GUIDs reais de entidades no banco.

---

**Rotas personalizadas implementadas e funcionando! 🎉**

