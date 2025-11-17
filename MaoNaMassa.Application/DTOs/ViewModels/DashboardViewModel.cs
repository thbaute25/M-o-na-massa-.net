namespace MaoNaMassa.Application.DTOs.ViewModels;

/// <summary>
/// ViewModel para dashboard do usuário
/// </summary>
public class DashboardViewModel
{
    public Guid UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public int TotalCursosInscritos { get; set; }
    public int TotalCursosCompletos { get; set; }
    public int TotalCertificados { get; set; }
    public int TotalServicosPublicados { get; set; }
    public int TotalAvaliacoesRecebidas { get; set; }
    public decimal? AvaliacaoMediaRecebida { get; set; }
    public List<CursoResumoViewModel> CursosEmAndamento { get; set; } = new();
    public List<CertificadoViewModel> CertificadosRecentes { get; set; } = new();
    public List<ServicoResumoViewModel> ServicosRecentes { get; set; } = new();
}

/// <summary>
/// ViewModel resumido para curso
/// </summary>
public class CursoResumoViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public decimal ProgressoPercentual { get; set; }
    public int AulasCompletas { get; set; }
    public int TotalAulas { get; set; }
}

/// <summary>
/// ViewModel resumido para serviço
/// </summary>
public class ServicoResumoViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public decimal? Preco { get; set; }
    public string PrecoFormatado { get; set; } = string.Empty;
    public decimal? AvaliacaoMedia { get; set; }
    public int TotalAvaliacoes { get; set; }
    public DateTime DataPublicacao { get; set; }
    public string DataPublicacaoFormatada { get; set; } = string.Empty;
}

