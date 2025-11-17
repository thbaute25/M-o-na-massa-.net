namespace MaoNaMassa.Application.DTOs.ViewModels;

/// <summary>
/// ViewModel para apresentação de dados do curso na interface
/// </summary>
public class CursoViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Nivel { get; set; } = string.Empty;
    public string NivelDisplay { get; set; } = string.Empty; // "Iniciante", "Intermediário", "Avançado"
    public DateTime DataCriacao { get; set; }
    public string DataCriacaoFormatada { get; set; } = string.Empty;
    public int TotalAulas { get; set; }
    public int TotalQuizzes { get; set; }
    public int TotalCertificadosEmitidos { get; set; }
    public string DuracaoEstimada { get; set; } = string.Empty; // "30 minutos", "1 hora", etc.
    public bool EstaInscrito { get; set; }
    public decimal? ProgressoPercentual { get; set; }
}

