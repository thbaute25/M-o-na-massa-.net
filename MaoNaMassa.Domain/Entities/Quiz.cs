using MaoNaMassa.Domain.Exceptions;

namespace MaoNaMassa.Domain.Entities;

public class Quiz
{
    public Guid Id { get; private set; }
    public Guid CursoId { get; private set; }
    public string Pergunta { get; private set; }
    public string RespostaCorreta { get; private set; }
    
    // Navegação
    public virtual Curso Curso { get; private set; } = null!;
    public virtual ICollection<RespostaQuiz> RespostasQuiz { get; private set; } = new List<RespostaQuiz>();

    // Construtor privado para EF Core
    private Quiz() { }

    public Quiz(
        Guid cursoId,
        string pergunta,
        string respostaCorreta)
    {
        Id = Guid.NewGuid();
        
        SetCursoId(cursoId);
        SetPergunta(pergunta);
        SetRespostaCorreta(respostaCorreta);
    }

    public void SetCursoId(Guid cursoId)
    {
        if (cursoId == Guid.Empty)
            throw new DomainException("ID do curso é obrigatório.");

        CursoId = cursoId;
    }

    public void SetPergunta(string pergunta)
    {
        if (string.IsNullOrWhiteSpace(pergunta))
            throw new DomainException("Pergunta do quiz é obrigatória.");
        
        if (pergunta.Length < 10)
            throw new DomainException("Pergunta deve ter no mínimo 10 caracteres.");
        
        if (pergunta.Length > 500)
            throw new DomainException("Pergunta deve ter no máximo 500 caracteres.");

        Pergunta = pergunta.Trim();
    }

    public void SetRespostaCorreta(string respostaCorreta)
    {
        if (string.IsNullOrWhiteSpace(respostaCorreta))
            throw new DomainException("Resposta correta é obrigatória.");
        
        if (respostaCorreta.Length > 200)
            throw new DomainException("Resposta correta deve ter no máximo 200 caracteres.");

        RespostaCorreta = respostaCorreta.Trim();
    }

    public bool VerificarResposta(string resposta)
    {
        if (string.IsNullOrWhiteSpace(resposta))
            return false;

        return RespostaCorreta.Equals(resposta.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}

