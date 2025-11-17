using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

/// <summary>
/// ViewModel para exibição de dados do usuário
/// </summary>
public class UsuarioViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Cidade")]
    public string Cidade { get; set; } = string.Empty;

    [Display(Name = "Área de Interesse")]
    public string AreaInteresse { get; set; } = string.Empty;

    [Display(Name = "Tipo de Usuário")]
    public string TipoUsuario { get; set; } = string.Empty;

    [Display(Name = "Data de Criação")]
    [DataType(DataType.Date)]
    public DateTime DataCriacao { get; set; }

    [Display(Name = "Tem Perfil Profissional")]
    public bool TemPerfilProfissional { get; set; }

    [Display(Name = "Total de Certificados")]
    public int TotalCertificados { get; set; }
}

/// <summary>
/// ViewModel para atualização de dados do usuário
/// </summary>
public class AtualizarUsuarioViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
    [Display(Name = "Nome Completo")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cidade é obrigatória")]
    [StringLength(100, ErrorMessage = "A cidade deve ter no máximo 100 caracteres")]
    [Display(Name = "Cidade")]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "A área de interesse é obrigatória")]
    [StringLength(100, ErrorMessage = "A área de interesse deve ter no máximo 100 caracteres")]
    [Display(Name = "Área de Interesse")]
    public string AreaInteresse { get; set; } = string.Empty;
}

/// <summary>
/// ViewModel para busca/filtro de usuários
/// </summary>
public class BuscarUsuarioViewModel
{
    [Display(Name = "Nome")]
    public string? Nome { get; set; }

    [Display(Name = "Cidade")]
    public string? Cidade { get; set; }

    [Display(Name = "Tipo de Usuário")]
    public string? TipoUsuario { get; set; }

    [Display(Name = "Página")]
    [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior que zero")]
    public int Pagina { get; set; } = 1;

    [Display(Name = "Itens por Página")]
    [Range(1, 100, ErrorMessage = "Os itens por página devem estar entre 1 e 100")]
    public int ItensPorPagina { get; set; } = 10;
}

