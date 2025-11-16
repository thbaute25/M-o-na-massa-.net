namespace MaoNaMassa.Application.DTOs;

public class QuizDTO
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string Pergunta { get; set; } = string.Empty;
    public string RespostaCorreta { get; set; } = string.Empty;
}

public class CreateQuizDTO
{
    public Guid CursoId { get; set; }
    public string Pergunta { get; set; } = string.Empty;
    public string RespostaCorreta { get; set; } = string.Empty;
}

public class ResponderQuizDTO
{
    public Guid QuizId { get; set; }
    public string Resposta { get; set; } = string.Empty;
}

public class ResultadoQuizDTO
{
    public Guid RespostaQuizId { get; set; }
    public bool Correta { get; set; }
    public string RespostaEnviada { get; set; } = string.Empty;
    public string RespostaCorreta { get; set; } = string.Empty;
}

