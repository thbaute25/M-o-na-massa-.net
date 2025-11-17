using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

public class CriarUsuarioViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
    [Display(Name = "Nome Completo")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(255, ErrorMessage = "O email deve ter no máximo 255 caracteres")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cidade é obrigatória")]
    [StringLength(100, ErrorMessage = "A cidade deve ter no máximo 100 caracteres")]
    [Display(Name = "Cidade")]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "A área de interesse é obrigatória")]
    [StringLength(100, ErrorMessage = "A área de interesse deve ter no máximo 100 caracteres")]
    [Display(Name = "Área de Interesse")]
    public string AreaInteresse { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo de usuário é obrigatório")]
    [Display(Name = "Tipo de Usuário")]
    public string TipoUsuario { get; set; } = string.Empty;
}

