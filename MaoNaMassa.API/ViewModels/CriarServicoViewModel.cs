using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

public class CriarServicoViewModel
{
    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "O título deve ter entre 5 e 200 caracteres")]
    [Display(Name = "Título do Serviço")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 1000 caracteres")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cidade é obrigatória")]
    [StringLength(100, ErrorMessage = "A cidade deve ter no máximo 100 caracteres")]
    [Display(Name = "Cidade")]
    public string Cidade { get; set; } = string.Empty;

    [Range(0, 999999.99, ErrorMessage = "O preço deve ser um valor positivo")]
    [Display(Name = "Preço (R$)")]
    [DataType(DataType.Currency)]
    public decimal? Preco { get; set; }

    [Required(ErrorMessage = "O ID do profissional é obrigatório")]
    [Display(Name = "ID do Profissional")]
    public Guid ProfissionalId { get; set; }
}

