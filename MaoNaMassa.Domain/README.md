# MaoNaMassa.Domain

## 📋 Entidades do Domínio

Este projeto contém todas as entidades de domínio do sistema Mão na Massa, implementadas com **invariantes** (regras de negócio) que garantem a integridade dos dados.

### Entidades Implementadas

1. **Usuario** - Representa os usuários do sistema
2. **Curso** - Cursos disponíveis para aprendizado
3. **Aula** - Etapas dentro de um curso
4. **Quiz** - Perguntas de avaliação dos cursos
5. **RespostaQuiz** - Respostas dos alunos aos quizzes
6. **Certificado** - Certificados de conclusão de curso
7. **Profissional** - Perfil profissional dos usuários
8. **Servico** - Serviços oferecidos pelos profissionais
9. **Avaliacao** - Avaliações dos serviços

### 🛡️ Invariantes Implementadas

#### Usuario
- Nome: obrigatório, mínimo 3 caracteres, máximo 100
- Email: obrigatório, formato válido, máximo 255 caracteres
- Senha: obrigatória, mínimo 6 caracteres
- Cidade: obrigatória, máximo 100 caracteres
- Área de Interesse: obrigatória, máximo 100 caracteres
- Tipo de Usuário: deve ser um dos valores válidos (Aprendiz, Cliente, Empresa, Profissional)

#### Curso
- Título: obrigatório, mínimo 5 caracteres, máximo 200
- Descrição: obrigatória, mínimo 10 caracteres, máximo 1000
- Área: obrigatória, máximo 100 caracteres
- Nível: deve ser Iniciante, Intermediário ou Avançado

#### Aula
- Título: obrigatório, mínimo 5 caracteres, máximo 200
- Conteúdo: obrigatório, mínimo 20 caracteres
- Ordem: deve ser maior que zero
- CursoId: obrigatório

#### Quiz
- Pergunta: obrigatória, mínimo 10 caracteres, máximo 500
- Resposta Correta: obrigatória, máximo 200 caracteres
- CursoId: obrigatório

#### RespostaQuiz
- Resposta: obrigatória, máximo 200 caracteres
- UsuarioId e QuizId: obrigatórios

#### Certificado
- Nota Final: entre 0 e 100
- Código de Certificado: gerado automaticamente
- Aprovação: nota >= 70

#### Profissional
- Descrição: obrigatória, mínimo 20 caracteres, máximo 1000
- Avaliação Média: entre 0 e 5 (quando presente)
- Disponibilidade: booleano

#### Servico
- Título: obrigatório, mínimo 5 caracteres, máximo 200
- Descrição: obrigatória, mínimo 20 caracteres, máximo 1000
- Cidade: obrigatória, máximo 100 caracteres
- Preço: não pode ser negativo (quando presente)

#### Avaliacao
- Nota: entre 1 e 5
- Comentário: máximo 500 caracteres (opcional)
- ServicoId e UsuarioId: obrigatórios

### 🏗️ Padrões de Design

- **Rich Domain Model**: Entidades com comportamento e validações
- **Encapsulamento**: Propriedades privadas com setters controlados
- **Factory Methods**: Métodos estáticos para criação (ex: `Certificado.Criar()`)
- **Domain Exceptions**: Exceções específicas do domínio

### 📝 Notas

- Todas as entidades possuem construtores privados para EF Core
- As validações são executadas através de métodos `Set*` que garantem invariantes
- As exceções de domínio são lançadas quando as regras são violadas

