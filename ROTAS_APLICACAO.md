# 🛣️ Rotas da Aplicação - Mão na Massa

## 📋 Rotas Configuradas

### Rotas Padrão (Razor Pages)

| Rota | Página | Descrição |
|------|--------|-----------|
| `/` | Index.cshtml | Redireciona para `/Home` |
| `/Home` | Home.cshtml | Página inicial da aplicação |
| `/Cursos` | Cursos.cshtml | Página de cursos |
| `/Servicos` | Servicos.cshtml | Página de serviços |
| `/Profissionais` | Profissionais.cshtml | Página de profissionais |

### Rotas da API (Controllers)

| Rota | Controller | Método | Descrição |
|------|------------|--------|-----------|
| `/api/usuarios` | UsuariosController | GET | Listar usuários |
| `/api/usuarios/{id}` | UsuariosController | GET | Buscar usuário |
| `/api/usuarios/search` | UsuariosController | GET | Buscar com filtros |
| `/api/cursos` | CursosController | GET | Listar cursos |
| `/api/cursos/{id}` | CursosController | GET | Buscar curso |
| `/api/cursos/search` | CursosController | GET | Buscar cursos |
| `/api/servicos` | ServicosController | GET | Listar serviços |
| `/api/servicos/{id}` | ServicosController | GET | Buscar serviço |
| `/api/servicos/search` | ServicosController | GET | Busca avançada |
| `/api/profissionais` | ProfissionaisController | GET | Listar profissionais |
| `/api/profissionais/{id}` | ProfissionaisController | GET | Buscar profissional |
| `/api/profissionais/search` | ProfissionaisController | GET | Buscar profissionais |
| `/api/health` | HealthController | GET | Health check |

### Rotas Especiais

| Rota | Descrição |
|------|-----------|
| `/swagger` | Documentação interativa da API (Swagger UI) |

---

## 🎯 Configuração de Rotas

### Program.cs

```csharp
// Adicionar suporte a Razor Pages
builder.Services.AddRazorPages();

// Configurar rotas
app.UseStaticFiles(); // Arquivos estáticos (CSS, JS)
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages(); // Mapear Razor Pages
app.MapControllers(); // Mapear API Controllers
```

### Rotas Personalizadas nas Páginas

As rotas são definidas diretamente nas páginas Razor usando a diretiva `@page`:

```razor
@page "/Home"  // Rota personalizada: /Home
@page "/Cursos"  // Rota personalizada: /Cursos
@page "/"  // Rota raiz: /
```

---

## 📝 Estrutura de Arquivos

```
MaoNaMassa.API/
├── Pages/
│   ├── Index.cshtml          → Rota: /
│   ├── Home.cshtml           → Rota: /Home
│   ├── Cursos.cshtml         → Rota: /Cursos
│   ├── Servicos.cshtml       → Rota: /Servicos
│   ├── Profissionais.cshtml  → Rota: /Profissionais
│   ├── Shared/
│   │   └── _Layout.cshtml    → Layout principal
│   ├── _ViewImports.cshtml   → Imports globais
│   └── _ViewStart.cshtml     → Configuração de layout
├── wwwroot/
│   └── css/
│       └── site.css          → Estilos customizados
└── Controllers/
    └── [Controllers da API]  → Rotas: /api/*
```

---

## ✅ Funcionalidades Implementadas

✅ **Rotas padrão** configuradas para páginas principais  
✅ **Rotas personalizadas** usando `@page` directive  
✅ **Layout Bootstrap** com navegação  
✅ **Arquivos estáticos** habilitados (CSS, JS)  
✅ **Integração** entre páginas Razor e API REST  
✅ **Navegação** entre páginas funcionando  

---

## 🚀 Como Testar

1. **Iniciar a aplicação:**
   ```powershell
   dotnet run --project MaoNaMassa.API
   ```

2. **Acessar as rotas:**
   - Página inicial: http://localhost:5136/
   - Home: http://localhost:5136/Home
   - Cursos: http://localhost:5136/Cursos
   - Serviços: http://localhost:5136/Servicos
   - Profissionais: http://localhost:5136/Profissionais
   - Swagger: http://localhost:5136/swagger
   - API: http://localhost:5136/api/health

---

**Rotas configuradas e funcionando! 🎉**

