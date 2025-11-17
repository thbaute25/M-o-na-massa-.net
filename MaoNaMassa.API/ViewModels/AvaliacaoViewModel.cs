using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

/// <summary>
/// ViewModel para exibição de dados da avaliação
/// </summary>
public class AvaliacaoViewModel
{
    public Guid Id { get; set; }
    public Guid ServicoId { get; set; }
    public Guid UsuarioId { get; set; }

    [Display(Name = "Nome do Usuário")]
    public string NomeUsuario { get; set; } = string.Empty;

    [Display(Name = "Título do Serviço")]
    public string TituloServico { get; set; } = string.Empty;

    [Display(Name = "Nota")]
    [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5")]
    public int Nota { get; set; }

    [Display(Name = "Comentário")]
    public string? Comentario { get; set; }

    [Display(Name = "Data")]
    [DataType(DataType.Date)]
    public DateTime Data { get; set; }
}

