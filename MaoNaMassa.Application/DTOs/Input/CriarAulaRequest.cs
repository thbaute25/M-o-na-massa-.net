namespace MaoNaMassa.Application.DTOs.Input;

/// <summary>
/// DTO de entrada para criar uma nova aula
/// </summary>
public class CriarAulaRequest
{
    public Guid CursoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public int Ordem { get; set; }
}

