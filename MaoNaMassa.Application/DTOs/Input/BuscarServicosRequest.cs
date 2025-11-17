namespace MaoNaMassa.Application.DTOs.Input;

/// <summary>
/// DTO de entrada para buscar serviços com filtros
/// </summary>
public class BuscarServicosRequest
{
    public string? Cidade { get; set; }
    public string? Termo { get; set; }
    public decimal? PrecoMaximo { get; set; }
    public decimal? AvaliacaoMinima { get; set; }
}

