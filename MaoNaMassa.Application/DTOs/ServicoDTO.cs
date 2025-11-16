namespace MaoNaMassa.Application.DTOs;

public class ServicoDTO
{
    public Guid Id { get; set; }
    public Guid ProfissionalId { get; set; }
    public string NomeProfissional { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public decimal? Preco { get; set; }
    public DateTime DataPublicacao { get; set; }
    public decimal? AvaliacaoMedia { get; set; }
    public int TotalAvaliacoes { get; set; }
}

public class CreateServicoDTO
{
    public Guid ProfissionalId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public decimal? Preco { get; set; }
}

public class BuscarServicoDTO
{
    public string? Cidade { get; set; }
    public string? Termo { get; set; }
    public decimal? PrecoMaximo { get; set; }
    public decimal? AvaliacaoMinima { get; set; }
}

