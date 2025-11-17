using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

public class ResponderQuizViewModel
{
    [Required(ErrorMessage = "O ID do usuário é obrigatório")]
    [Display(Name = "ID do Usuário")]
    public Guid UsuarioId { get; set; }

    [Required(ErrorMessage = "O ID do quiz é obrigatório")]
    [Display(Name = "ID do Quiz")]
    public Guid QuizId { get; set; }

    [Required(ErrorMessage = "A resposta é obrigatória")]
    [StringLength(200, ErrorMessage = "A resposta deve ter no máximo 200 caracteres")]
    [Display(Name = "Sua Resposta")]
    public string Resposta { get; set; } = string.Empty;

    // Propriedades para exibição
    public string? Pergunta { get; set; }
    public string? CursoTitulo { get; set; }
}

