namespace MaoNaMassa.Application.DTOs.Output;

/// <summary>
/// DTO de saída para informações do serviço
/// </summary>
public class ServicoResponse
{
    public Guid Id { get; set; }
    public Guid ProfissionalId { get; set; }
    public string NomeProfissional { get; set; } = string.Empty;
    public string CidadeProfissional { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public decimal? Preco { get; set; }
    public DateTime DataPublicacao { get; set; }
    public decimal? AvaliacaoMedia { get; set; }
    public int TotalAvaliacoes { get; set; }
    public List<AvaliacaoBasicaResponse> UltimasAvaliacoes { get; set; } = new();
}

