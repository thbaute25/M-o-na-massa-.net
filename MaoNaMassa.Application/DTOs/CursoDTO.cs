namespace MaoNaMassa.Application.DTOs;

public class CursoDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Nivel { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public int TotalAulas { get; set; }
    public int TotalQuizzes { get; set; }
    public int TotalCertificadosEmitidos { get; set; }
}

public class CreateCursoDTO
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Nivel { get; set; } = string.Empty;
}

public class CursoCompletoDTO : CursoDTO
{
    public List<AulaDTO> Aulas { get; set; } = new();
    public List<QuizDTO> Quizzes { get; set; } = new();
}

