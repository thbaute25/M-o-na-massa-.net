namespace MaoNaMassa.Application.DTOs.ViewModels;

/// <summary>
/// ViewModel para apresentação de dados do serviço na interface
/// </summary>
public class ServicoViewModel
{
    public Guid Id { get; set; }
    public Guid ProfissionalId { get; set; }
    public string NomeProfissional { get; set; } = string.Empty;
    public string CidadeProfissional { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public decimal? Preco { get; set; }
    public string PrecoFormatado { get; set; } = string.Empty; // "R$ 150,00"
    public DateTime DataPublicacao { get; set; }
    public string DataPublicacaoFormatada { get; set; } = string.Empty; // "há 2 dias"
    public decimal? AvaliacaoMedia { get; set; }
    public string AvaliacaoMediaFormatada { get; set; } = string.Empty; // "4.5 ⭐"
    public int TotalAvaliacoes { get; set; }
    public string TotalAvaliacoesFormatado { get; set; } = string.Empty; // "(15 avaliações)"
    public bool Disponivel { get; set; }
    public string StatusDisponibilidade { get; set; } = string.Empty; // "Disponível", "Indisponível"
}

