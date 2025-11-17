using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

/// <summary>
/// ViewModel para exibição de dados do serviço
/// </summary>
public class ServicoViewModel
{
    public Guid Id { get; set; }
    public Guid ProfissionalId { get; set; }

    [Display(Name = "Profissional")]
    public string NomeProfissional { get; set; } = string.Empty;

    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Display(Name = "Cidade")]
    public string Cidade { get; set; } = string.Empty;

    [Display(Name = "Preço")]
    [DataType(DataType.Currency)]
    public decimal? Preco { get; set; }

    [Display(Name = "Data de Publicação")]
    [DataType(DataType.Date)]
    public DateTime DataPublicacao { get; set; }

    [Display(Name = "Avaliação Média")]
    [DisplayFormat(DataFormatString = "{0:F2}")]
    public decimal? AvaliacaoMedia { get; set; }

    [Display(Name = "Total de Avaliações")]
    public int TotalAvaliacoes { get; set; }
}

/// <summary>
/// ViewModel para busca/filtro de serviços
/// </summary>
public class BuscarServicoViewModel
{
    [Display(Name = "Cidade")]
    public string? Cidade { get; set; }

    [Display(Name = "Termo de Busca")]
    public string? Termo { get; set; }

    [Display(Name = "Preço Máximo")]
    [DataType(DataType.Currency)]
    [Range(0, 999999.99, ErrorMessage = "O preço máximo deve ser um valor positivo")]
    public decimal? PrecoMaximo { get; set; }

    [Display(Name = "Avaliação Mínima")]
    [Range(0, 5, ErrorMessage = "A avaliação mínima deve estar entre 0 e 5")]
    public decimal? AvaliacaoMinima { get; set; }

    [Display(Name = "Página")]
    [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior que zero")]
    public int Pagina { get; set; } = 1;

    [Display(Name = "Itens por Página")]
    [Range(1, 100, ErrorMessage = "Os itens por página devem estar entre 1 e 100")]
    public int ItensPorPagina { get; set; } = 10;

    [Display(Name = "Ordenar por")]
    public string? OrdenarPor { get; set; } = "AvaliacaoMedia";

    [Display(Name = "Ordem")]
    public string? Ordem { get; set; } = "desc";
}

