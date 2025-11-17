using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

public class CriarPerfilProfissionalViewModel
{
    [Required(ErrorMessage = "O ID do usuário é obrigatório")]
    [Display(Name = "ID do Usuário")]
    public Guid UsuarioId { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 1000 caracteres")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Descrição do Perfil")]
    public string Descricao { get; set; } = string.Empty;
}

