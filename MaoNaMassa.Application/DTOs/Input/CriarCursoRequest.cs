namespace MaoNaMassa.Application.DTOs.Input;

/// <summary>
/// DTO de entrada para criar um novo curso
/// </summary>
public class CriarCursoRequest
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Nivel { get; set; } = string.Empty;
}

