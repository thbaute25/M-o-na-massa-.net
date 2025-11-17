namespace MaoNaMassa.Application.DTOs.Input;

/// <summary>
/// DTO de entrada para avaliar um serviço
/// </summary>
public class AvaliarServicoRequest
{
    public Guid ServicoId { get; set; }
    public int Nota { get; set; }
    public string? Comentario { get; set; }
}

