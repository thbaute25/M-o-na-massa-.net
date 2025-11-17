namespace MaoNaMassa.Application.DTOs.Output;

/// <summary>
/// DTO de saída para informações da avaliação
/// </summary>
public class AvaliacaoResponse
{
    public Guid Id { get; set; }
    public Guid ServicoId { get; set; }
    public Guid UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public DateTime Data { get; set; }
}

/// <summary>
/// DTO básico de avaliação para listagem
/// </summary>
public class AvaliacaoBasicaResponse
{
    public Guid Id { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public DateTime Data { get; set; }
}

