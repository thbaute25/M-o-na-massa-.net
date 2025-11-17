namespace MaoNaMassa.Application.DTOs.Input;

/// <summary>
/// DTO de entrada para criar um novo serviço
/// </summary>
public class CriarServicoRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public decimal? Preco { get; set; }
}

