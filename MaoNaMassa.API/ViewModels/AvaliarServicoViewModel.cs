using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

public class AvaliarServicoViewModel
{
    [Required(ErrorMessage = "O ID do usuário é obrigatório")]
    [Display(Name = "ID do Usuário")]
    public Guid UsuarioId { get; set; }

    [Required(ErrorMessage = "O ID do serviço é obrigatório")]
    [Display(Name = "ID do Serviço")]
    public Guid ServicoId { get; set; }

    [Required(ErrorMessage = "A nota é obrigatória")]
    [Range(1, 5, ErrorMessage = "A nota deve ser entre 1 e 5")]
    [Display(Name = "Nota (1 a 5)")]
    public int Nota { get; set; }

    [StringLength(500, ErrorMessage = "O comentário deve ter no máximo 500 caracteres")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Comentário (opcional)")]
    public string? Comentario { get; set; }

    // Propriedades para exibição
    public string? ServicoTitulo { get; set; }
    public string? ProfissionalNome { get; set; }
}

