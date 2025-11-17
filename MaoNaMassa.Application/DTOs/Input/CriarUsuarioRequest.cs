namespace MaoNaMassa.Application.DTOs.Input;

/// <summary>
/// DTO de entrada para criar um novo usuário
/// </summary>
public class CriarUsuarioRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string AreaInteresse { get; set; } = string.Empty;
    public string TipoUsuario { get; set; } = string.Empty;
}

