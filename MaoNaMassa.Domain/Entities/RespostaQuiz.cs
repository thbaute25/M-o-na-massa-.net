using MaoNaMassa.Domain.Exceptions;

namespace MaoNaMassa.Domain.Entities;

public class RespostaQuiz
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid QuizId { get; private set; }
    public string Resposta { get; private set; }
    public bool Correta { get; private set; }
    public DateTime DataResposta { get; private set; }
    
    // Navegação
    public virtual Usuario Usuario { get; private set; } = null!;
    public virtual Quiz Quiz { get; private set; } = null!;

    // Construtor privado para EF Core
    private RespostaQuiz() { }

    public RespostaQuiz(
        Guid usuarioId,
        Guid quizId,
        string resposta,
        bool correta)
    {
        Id = Guid.NewGuid();
        DataResposta = DateTime.UtcNow;
        
        SetUsuarioId(usuarioId);
        SetQuizId(quizId);
        SetResposta(resposta);
        Correta = correta;
    }

    public void SetUsuarioId(Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            throw new DomainException("ID do usuário é obrigatório.");

        UsuarioId = usuarioId;
    }

    public void SetQuizId(Guid quizId)
    {
        if (quizId == Guid.Empty)
            throw new DomainException("ID do quiz é obrigatório.");

        QuizId = quizId;
    }

    public void SetResposta(string resposta)
    {
        if (string.IsNullOrWhiteSpace(resposta))
            throw new DomainException("Resposta é obrigatória.");
        
        if (resposta.Length > 200)
            throw new DomainException("Resposta deve ter no máximo 200 caracteres.");

        Resposta = resposta.Trim();
    }

    public static RespostaQuiz Criar(Usuario usuario, Quiz quiz, string resposta)
    {
        if (usuario == null)
            throw new DomainException("Usuário não pode ser nulo.");
        
        if (quiz == null)
            throw new DomainException("Quiz não pode ser nulo.");

        var correta = quiz.VerificarResposta(resposta);
        
        return new RespostaQuiz(usuario.Id, quiz.Id, resposta, correta);
    }
}

