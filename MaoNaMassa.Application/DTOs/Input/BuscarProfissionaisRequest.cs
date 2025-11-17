namespace MaoNaMassa.Application.DTOs.Input;

/// <summary>
/// DTO de entrada para buscar profissionais com filtros
/// </summary>
public class BuscarProfissionaisRequest
{
    public string? Cidade { get; set; }
    public string? AreaInteresse { get; set; }
    public decimal? AvaliacaoMinima { get; set; }
    public bool ApenasDisponiveis { get; set; } = true;
}

