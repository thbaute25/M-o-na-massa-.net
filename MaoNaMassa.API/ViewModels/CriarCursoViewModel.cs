using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

public class CriarCursoViewModel
{
    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "O título deve ter entre 5 e 200 caracteres")]
    [Display(Name = "Título do Curso")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 1000 caracteres")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A área é obrigatória")]
    [StringLength(100, ErrorMessage = "A área deve ter no máximo 100 caracteres")]
    [Display(Name = "Área")]
    public string Area { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nível é obrigatório")]
    [Display(Name = "Nível")]
    public string Nivel { get; set; } = string.Empty;
}

