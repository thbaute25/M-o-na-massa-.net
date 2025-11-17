namespace MaoNaMassa.Application.DTOs.Output;

/// <summary>
/// DTO de saída para informações do usuário
/// </summary>
public class UsuarioResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string AreaInteresse { get; set; } = string.Empty;
    public string TipoUsuario { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public bool TemPerfilProfissional { get; set; }
    public int TotalCertificados { get; set; }
}

