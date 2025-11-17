using MaoNaMassa.Application.DTOs.Output;

namespace MaoNaMassa.Application.DTOs.Output;

/// <summary>
/// DTO de saída para curso completo com aulas e quizzes
/// </summary>
public class CursoCompletoResponse : CursoResponse
{
    public List<AulaResponse> Aulas { get; set; } = new();
    public List<QuizResponse> Quizzes { get; set; } = new();
    public ProgressoCursoResponse? Progresso { get; set; }
}

/// <summary>
/// DTO de saída para progresso do curso do usuário
/// </summary>
public class ProgressoCursoResponse
{
    public int QuizzesRespondidos { get; set; }
    public int TotalQuizzes { get; set; }
    public int RespostasCorretas { get; set; }
    public decimal PercentualConclusao { get; set; }
    public bool PossuiCertificado { get; set; }
    public decimal? NotaAtual { get; set; }
}

