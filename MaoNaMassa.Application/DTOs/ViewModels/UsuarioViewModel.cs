namespace MaoNaMassa.Application.DTOs.ViewModels;

/// <summary>
/// ViewModel para apresentação de dados do usuário na interface
/// </summary>
public class UsuarioViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string AreaInteresse { get; set; } = string.Empty;
    public string TipoUsuario { get; set; } = string.Empty;
    public string TipoUsuarioDisplay { get; set; } = string.Empty; // Para exibição formatada
    public DateTime DataCriacao { get; set; }
    public string DataCriacaoFormatada { get; set; } = string.Empty;
    public bool TemPerfilProfissional { get; set; }
    public int TotalCertificados { get; set; }
    public string StatusPerfil { get; set; } = string.Empty; // "Ativo", "Inativo", "Pendente"
}

