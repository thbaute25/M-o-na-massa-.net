namespace MaoNaMassa.Application.DTOs.Output;

/// <summary>
/// DTO de saída para informações do curso
/// </summary>
public class CursoResponse
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

