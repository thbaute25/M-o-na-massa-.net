using MaoNaMassa.Domain.Exceptions;

namespace MaoNaMassa.Domain.Entities;

public class Curso
{
    public Guid Id { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public string Area { get; private set; }
    public string Nivel { get; private set; }
    public DateTime DataCriacao { get; private set; }
    
    // Navegação
    public virtual ICollection<Aula> Aulas { get; private set; } = new List<Aula>();
    public virtual ICollection<Quiz> Quizzes { get; private set; } = new List<Quiz>();
    public virtual ICollection<Certificado> Certificados { get; private set; } = new List<Certificado>();

    // Construtor privado para EF Core
    private Curso() { }

    public Curso(
        string titulo,
        string descricao,
        string area,
        string nivel)
    {
        Id = Guid.NewGuid();
        DataCriacao = DateTime.UtcNow;
        
        SetTitulo(titulo);
        SetDescricao(descricao);
        SetArea(area);
        SetNivel(nivel);
    }

    public void SetTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Título do curso é obrigatório.");
        
        if (titulo.Length < 5)
            throw new DomainException("Título deve ter no mínimo 5 caracteres.");
        
        if (titulo.Length > 200)
            throw new DomainException("Título deve ter no máximo 200 caracteres.");

        Titulo = titulo.Trim();
    }

    public void SetDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição do curso é obrigatória.");
        
        if (descricao.Length < 10)
            throw new DomainException("Descrição deve ter no mínimo 10 caracteres.");
        
        if (descricao.Length > 1000)
            throw new DomainException("Descrição deve ter no máximo 1000 caracteres.");

        Descricao = descricao.Trim();
    }

    public void SetArea(string area)
    {
        if (string.IsNullOrWhiteSpace(area))
            throw new DomainException("Área do curso é obrigatória.");
        
        if (area.Length > 100)
            throw new DomainException("Área deve ter no máximo 100 caracteres.");

        Area = area.Trim();
    }

    public void SetNivel(string nivel)
    {
        if (string.IsNullOrWhiteSpace(nivel))
            throw new DomainException("Nível do curso é obrigatório.");
        
        var niveisValidos = new[] { "Iniciante", "Intermediário", "Avançado" };
        if (!niveisValidos.Contains(nivel))
            throw new DomainException($"Nível inválido. Níveis válidos: {string.Join(", ", niveisValidos)}.");

        Nivel = nivel;
    }

    public void AdicionarAula(Aula aula)
    {
        if (aula == null)
            throw new DomainException("Aula não pode ser nula.");
        
        if (aula.CursoId != Id)
            throw new DomainException("Aula não pertence a este curso.");

        Aulas.Add(aula);
    }

    public void AdicionarQuiz(Quiz quiz)
    {
        if (quiz == null)
            throw new DomainException("Quiz não pode ser nulo.");
        
        if (quiz.CursoId != Id)
            throw new DomainException("Quiz não pertence a este curso.");

        Quizzes.Add(quiz);
    }

    public bool PossuiAulas()
    {
        return Aulas.Any();
    }

    public bool PossuiQuizzes()
    {
        return Quizzes.Any();
    }
}

