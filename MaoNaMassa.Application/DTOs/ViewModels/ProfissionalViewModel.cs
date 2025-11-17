namespace MaoNaMassa.Application.DTOs.ViewModels;

/// <summary>
/// ViewModel para apresentação de dados do profissional na interface
/// </summary>
public class ProfissionalViewModel
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string EmailUsuario { get; set; } = string.Empty;
    public string CidadeUsuario { get; set; } = string.Empty;
    public string AreaInteresse { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal? AvaliacaoMedia { get; set; }
    public string AvaliacaoMediaFormatada { get; set; } = string.Empty; // "4.5 ⭐"
    public bool Disponivel { get; set; }
    public string StatusDisponibilidade { get; set; } = string.Empty; // "Disponível", "Indisponível"
    public int TotalServicos { get; set; }
    public int TotalAvaliacoes { get; set; }
    public string TotalAvaliacoesFormatado { get; set; } = string.Empty;
    public List<CertificadoViewModel> Certificados { get; set; } = new();
    public string Experiencia { get; set; } = string.Empty; // "5+ anos de experiência"
}

/// <summary>
/// ViewModel para certificado na listagem
/// </summary>
public class CertificadoViewModel
{
    public Guid Id { get; set; }
    public string TituloCurso { get; set; } = string.Empty;
    public decimal NotaFinal { get; set; }
    public string NotaFormatada { get; set; } = string.Empty; // "85/100"
    public DateTime DataConclusao { get; set; }
    public string DataConclusaoFormatada { get; set; } = string.Empty;
    public bool Aprovado { get; set; }
    public string Status { get; set; } = string.Empty; // "Aprovado", "Reprovado"
}

