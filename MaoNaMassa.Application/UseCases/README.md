# Casos de Uso (Use Cases)

Este diretório contém os **casos de uso** claros e específicos do sistema Mão na Massa. Cada caso de uso representa uma funcionalidade completa e bem definida.

## 📋 Casos de Uso Implementados

### 1. **CompletarCursoUseCase**
**Descrição:** Permite que um usuário complete um curso respondendo todos os quizzes e obtenha um certificado.

**Fluxo:**
1. Validar usuário e curso
2. Verificar se já possui certificado
3. Validar se curso possui quizzes
4. Verificar se usuário respondeu todos os quizzes
5. Calcular nota final: `(respostas corretas / total quizzes) * 100`
6. Validar nota mínima (>= 70)
7. Gerar certificado

**Entrada:** `usuarioId`, `cursoId`  
**Saída:** `CertificadoDTO`

---

### 2. **ResponderQuizUseCase**
**Descrição:** Permite que um usuário responda um quiz de um curso.

**Fluxo:**
1. Validar usuário e quiz
2. Validar curso do quiz
3. Verificar se usuário já respondeu (prevenção de duplicatas)
4. Criar resposta do quiz
5. Retornar resultado (correto/incorreto)

**Entrada:** `usuarioId`, `ResponderQuizDTO`  
**Saída:** `ResultadoQuizDTO`

---

### 3. **CriarPerfilProfissionalUseCase**
**Descrição:** Permite que um usuário crie seu perfil profissional para oferecer serviços.

**Fluxo:**
1. Validar usuário
2. Verificar se já possui perfil (prevenção de duplicatas)
3. Validar tipo de usuário (deve ser "Profissional" ou "Aprendiz")
4. Verificar certificados (recomendação)
5. Criar perfil profissional

**Entrada:** `CreateProfissionalDTO`  
**Saída:** `ProfissionalDTO`

---

### 4. **BuscarProfissionaisUseCase**
**Descrição:** Permite buscar profissionais disponíveis com filtros específicos.

**Fluxo:**
1. Buscar profissionais (disponíveis ou todos)
2. Aplicar filtros:
   - Cidade
   - Área de interesse
   - Avaliação mínima
3. Calcular estatísticas (total serviços, avaliações)
4. Ordenar por avaliação média e total de avaliações

**Entrada:** `cidade?`, `areaInteresse?`, `avaliacaoMinima?`, `apenasDisponiveis`  
**Saída:** `IEnumerable<ProfissionalDTO>`

---

### 5. **AvaliarServicoUseCase**
**Descrição:** Permite que um usuário avalie um serviço prestado por um profissional.

**Fluxo:**
1. Validar usuário e serviço
2. Validar profissional do serviço
3. **Regra:** Usuário não pode avaliar seu próprio serviço
4. Verificar se já avaliou (prevenção de duplicatas)
5. Criar avaliação
6. Atualizar avaliação média do serviço
7. Recalcular avaliação média do profissional

**Entrada:** `CreateAvaliacaoDTO`  
**Saída:** `AvaliacaoDTO`

---

### 6. **BuscarServicosUseCase**
**Descrição:** Permite buscar serviços oferecidos pelos profissionais com diversos filtros.

**Fluxo:**
1. Buscar serviços (cidade, termo)
2. Filtrar por profissional disponível
3. Aplicar filtros adicionais:
   - Preço máximo
   - Avaliação mínima
4. Ordenar por avaliação média, total de avaliações e data

**Entrada:** `BuscarServicoDTO`  
**Saída:** `IEnumerable<ServicoDTO>`

---

### 7. **CriarServicoUseCase**
**Descrição:** Permite que um profissional crie um novo serviço para oferecer.

**Fluxo:**
1. Validar profissional
2. **Regra:** Profissional deve estar disponível
3. Criar serviço
4. Mapear para DTO

**Entrada:** `CreateServicoDTO`  
**Saída:** `ServicoDTO`

---

### 8. **VisualizarCursoCompletoUseCase**
**Descrição:** Permite visualizar um curso com todas as suas aulas e quizzes.

**Fluxo:**
1. Validar curso
2. Buscar aulas (ordenadas)
3. Buscar quizzes
4. Se usuário fornecido, calcular progresso:
   - Quizzes respondidos
   - Respostas corretas
   - Possui certificado
5. Retornar curso completo com progresso

**Entrada:** `cursoId`, `usuarioId?`  
**Saída:** `CursoCompletoDTO`

---

## 🎯 Princípios dos Casos de Uso

### Single Responsibility
Cada caso de uso tem uma responsabilidade única e bem definida.

### Clear Intent
O nome do caso de uso deixa claro o que ele faz:
- `CompletarCursoUseCase` → Completa um curso
- `ResponderQuizUseCase` → Responde um quiz
- `BuscarProfissionaisUseCase` → Busca profissionais

### Encapsulation
Cada caso de uso encapsula toda a lógica necessária para executar a funcionalidade.

### Validation
Todos os casos de uso validam:
- Existência de entidades
- Regras de negócio
- Prevenção de duplicatas
- Estados válidos

### Error Handling
Exceções de domínio são lançadas quando regras são violadas.

---

## 📊 Comparação: Services vs Use Cases

### Services (Camada Services/)
- Operações mais genéricas
- Podem ser reutilizados por múltiplos casos de uso
- Exemplo: `CertificadoService`, `QuizService`

### Use Cases (Camada UseCases/)
- Funcionalidades específicas do negócio
- Representam ações do usuário
- Exemplo: `CompletarCursoUseCase`, `ResponderQuizUseCase`

---

## 🔄 Fluxo de Uso

```
Controller → Use Case → Services → Repositories → Database
```

1. **Controller** recebe requisição HTTP
2. **Use Case** orquestra a lógica de negócio
3. **Services** executam operações específicas
4. **Repositories** acessam dados
5. **Database** persiste informações

---

## 📝 Próximos Passos

- Implementar Controllers que utilizam os Use Cases
- Adicionar validações adicionais se necessário
- Criar testes unitários para cada caso de uso
- Documentar exemplos de uso

