# MaoNaMassa.Infrastructure

Camada de infraestrutura do sistema Mão na Massa, responsável por acesso a dados e integrações externas.

## 🗄️ Entity Framework Core - Mapeamentos

### Estrutura

```
MaoNaMassa.Infrastructure/
├── Data/
│   ├── ApplicationDbContext.cs          # DbContext principal
│   └── Configurations/
│       ├── UsuarioConfiguration.cs
│       ├── CursoConfiguration.cs
│       ├── AulaConfiguration.cs
│       ├── QuizConfiguration.cs
│       ├── RespostaQuizConfiguration.cs
│       ├── CertificadoConfiguration.cs
│       ├── ProfissionalConfiguration.cs
│       ├── ServicoConfiguration.cs
│       └── AvaliacaoConfiguration.cs
```

### Configurações Implementadas

#### 1. **UsuarioConfiguration**
- **Tabela:** `Usuarios`
- **Índices:**
  - `IX_Usuarios_Email` (único)
  - `IX_Usuarios_TipoUsuario`
  - `IX_Usuarios_Cidade`
- **Relacionamentos:**
  - 1:1 com `Profissional`
  - 1:N com `RespostaQuiz` (Cascade)
  - 1:N com `Certificado` (Cascade)
  - 1:N com `Avaliacao` (Restrict)

#### 2. **CursoConfiguration**
- **Tabela:** `Cursos`
- **Índices:**
  - `IX_Cursos_Area`
  - `IX_Cursos_Nivel`
- **Relacionamentos:**
  - 1:N com `Aula` (Cascade)
  - 1:N com `Quiz` (Cascade)
  - 1:N com `Certificado` (Restrict)

#### 3. **AulaConfiguration**
- **Tabela:** `Aulas`
- **Índices:**
  - `IX_Aulas_CursoId`
  - `IX_Aulas_CursoId_Ordem` (único composto)
- **Relacionamentos:**
  - N:1 com `Curso` (Cascade)

#### 4. **QuizConfiguration**
- **Tabela:** `Quizzes`
- **Índices:**
  - `IX_Quizzes_CursoId`
- **Relacionamentos:**
  - N:1 com `Curso` (Cascade)
  - 1:N com `RespostaQuiz` (Cascade)

#### 5. **RespostaQuizConfiguration**
- **Tabela:** `RespostasQuiz`
- **Índices:**
  - `IX_RespostasQuiz_UsuarioId`
  - `IX_RespostasQuiz_QuizId`
  - `IX_RespostasQuiz_UsuarioId_QuizId` (único composto - um usuário só pode responder um quiz uma vez)
- **Relacionamentos:**
  - N:1 com `Usuario` (Cascade)
  - N:1 com `Quiz` (Cascade)

#### 6. **CertificadoConfiguration**
- **Tabela:** `Certificados`
- **Índices:**
  - `IX_Certificados_UsuarioId`
  - `IX_Certificados_CursoId`
  - `IX_Certificados_CodigoCertificado` (único)
  - `IX_Certificados_UsuarioId_CursoId` (único composto - um usuário só pode ter um certificado por curso)
- **Relacionamentos:**
  - N:1 com `Usuario` (Cascade)
  - N:1 com `Curso` (Restrict)

#### 7. **ProfissionalConfiguration**
- **Tabela:** `Profissionais`
- **Índices:**
  - `IX_Profissionais_UsuarioId` (único)
  - `IX_Profissionais_Disponivel`
- **Relacionamentos:**
  - 1:1 com `Usuario` (Restrict)
  - 1:N com `Servico` (Cascade)

#### 8. **ServicoConfiguration**
- **Tabela:** `Servicos`
- **Índices:**
  - `IX_Servicos_ProfissionalId`
  - `IX_Servicos_Cidade`
  - `IX_Servicos_DataPublicacao`
- **Relacionamentos:**
  - N:1 com `Profissional` (Cascade)
  - 1:N com `Avaliacao` (Cascade)

#### 9. **AvaliacaoConfiguration**
- **Tabela:** `Avaliacoes`
- **Índices:**
  - `IX_Avaliacoes_ServicoId`
  - `IX_Avaliacoes_UsuarioId`
  - `IX_Avaliacoes_Data`
  - `IX_Avaliacoes_UsuarioId_ServicoId` (único composto - um usuário só pode avaliar um serviço uma vez)
- **Relacionamentos:**
  - N:1 com `Servico` (Cascade)
  - N:1 com `Usuario` (Restrict)

### Características dos Mapeamentos

#### Tipos de Dados
- **Guid:** Para IDs
- **string:** Com tamanhos máximos definidos
- **decimal:** Para valores monetários e notas (com precisão definida)
- **DateTime:** Para datas
- **bool:** Para flags booleanas

#### Constraints Implementadas
- **Índices únicos:** Email de usuário, código de certificado, relacionamentos 1:1
- **Índices compostos únicos:** Prevenção de duplicatas (resposta quiz, certificado, avaliação)
- **Foreign Keys:** Todos os relacionamentos configurados
- **Cascade Delete:** Para relacionamentos onde faz sentido (ex: deletar curso deleta aulas)
- **Restrict Delete:** Para relacionamentos críticos (ex: não deletar curso se houver certificados)

#### Validações no Banco
- Tamanhos máximos de strings
- Precisão de decimais
- Campos obrigatórios (IsRequired)
- Valores padrão (ex: `Disponivel = true`)

### ApplicationDbContext

O `ApplicationDbContext` utiliza `ApplyConfigurationsFromAssembly` para aplicar automaticamente todas as configurações do assembly, mantendo o código limpo e organizado.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
}
```

### Próximos Passos

1. Configurar connection string no `appsettings.json`
2. Criar migrations do EF Core
3. Implementar repositórios usando o DbContext
4. Configurar Unit of Work

