namespace MaoNaMassa.Application.DTOs.Input;

/// <summary>
/// DTO de entrada para atualizar dados do usuário
/// </summary>
public class AtualizarUsuarioRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string AreaInteresse { get; set; } = string.Empty;
}

