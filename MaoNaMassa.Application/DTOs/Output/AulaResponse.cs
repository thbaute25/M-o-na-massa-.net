namespace MaoNaMassa.Application.DTOs.Output;

/// <summary>
/// DTO de saída para informações da aula
/// </summary>
public class AulaResponse
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public int Ordem { get; set; }
}

