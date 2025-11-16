# MaoNaMassa.Application

## 📋 Regras de Negócio Implementadas

Este projeto contém os **Services** com todas as regras de negócio complexas do sistema Mão na Massa.

### 🎯 Services Implementados

#### 1. **CertificadoService**
**Regras de Negócio:**
- ✅ Validar se usuário e curso existem
- ✅ Verificar se usuário já possui certificado do curso (não permite duplicatas)
- ✅ Validar se curso possui quizzes para avaliação
- ✅ Calcular nota final: `(respostas corretas / total de quizzes) * 100`
- ✅ Criar certificado apenas se nota >= 70
- ✅ Gerar código único de certificado automaticamente

#### 2. **QuizService**
**Regras de Negócio:**
- ✅ Validar se usuário e quiz existem
- ✅ Validar se quiz pertence a um curso válido
- ✅ Impedir que usuário responda o mesmo quiz duas vezes
- ✅ Verificar resposta automaticamente e retornar resultado

#### 3. **AvaliacaoService**
**Regras de Negócio:**
- ✅ Validar se usuário e serviço existem
- ✅ **Usuário não pode avaliar seu próprio serviço**
- ✅ Impedir avaliação duplicada (usuário só pode avaliar uma vez)
- ✅ Atualizar avaliação média do serviço automaticamente
- ✅ Recalcular avaliação média do profissional quando nova avaliação é criada

#### 4. **ProfissionalService**
**Regras de Negócio:**
- ✅ Validar se usuário existe
- ✅ Impedir criação de perfil duplicado
- ✅ Validar tipo de usuário (deve ser "Profissional" ou "Aprendiz")
- ✅ Calcular total de serviços e avaliações

#### 5. **ServicoService**
**Regras de Negócio:**
- ✅ Validar se profissional existe
- ✅ Profissional deve estar disponível para criar novos serviços
- ✅ Buscar serviços com filtros (cidade, termo, preço máximo, avaliação mínima)
- ✅ Ordenar resultados por avaliação média e data de publicação

### 📦 DTOs (Data Transfer Objects)

Todos os DTOs foram criados para comunicação entre camadas:
- `UsuarioDTO`, `CreateUsuarioDTO`, `UpdateUsuarioDTO`
- `CursoDTO`, `CreateCursoDTO`, `CursoCompletoDTO`
- `AulaDTO`, `CreateAulaDTO`
- `QuizDTO`, `CreateQuizDTO`, `ResponderQuizDTO`, `ResultadoQuizDTO`
- `CertificadoDTO`, `GerarCertificadoDTO`
- `ProfissionalDTO`, `CreateProfissionalDTO`
- `ServicoDTO`, `CreateServicoDTO`, `BuscarServicoDTO`
- `AvaliacaoDTO`, `CreateAvaliacaoDTO`

### 🏗️ Interfaces de Repositório

Todas as interfaces foram criadas no Domain:
- `IUsuarioRepository`
- `ICursoRepository`
- `IAulaRepository`
- `IQuizRepository`
- `IRespostaQuizRepository`
- `ICertificadoRepository`
- `IProfissionalRepository`
- `IServicoRepository`
- `IAvaliacaoRepository`

### 🔒 Validações e Regras Implementadas

1. **Validação de Existência**: Todas as operações validam se entidades relacionadas existem
2. **Prevenção de Duplicatas**: Certificados, avaliações e perfis profissionais não podem ser duplicados
3. **Cálculos Automáticos**: Notas finais, médias de avaliação calculadas automaticamente
4. **Regras de Negócio Complexas**: 
   - Usuário não pode avaliar próprio serviço
   - Certificado só é gerado com nota mínima
   - Profissional deve estar disponível para criar serviços
5. **Validações de Estado**: Verificações de disponibilidade, tipo de usuário, etc.

### 📝 Próximos Passos

- Implementar repositórios na camada Infrastructure
- Criar DbContext e configurações do Entity Framework
- Implementar Controllers na API
- Adicionar tratamento de erros global

