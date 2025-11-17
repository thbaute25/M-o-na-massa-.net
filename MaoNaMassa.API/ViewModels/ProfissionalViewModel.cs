using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

/// <summary>
/// ViewModel para exibição de dados do profissional
/// </summary>
public class ProfissionalViewModel
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }

    [Display(Name = "Nome")]
    public string NomeUsuario { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string EmailUsuario { get; set; } = string.Empty;

    [Display(Name = "Cidade")]
    public string CidadeUsuario { get; set; } = string.Empty;

    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Display(Name = "Avaliação Média")]
    [DisplayFormat(DataFormatString = "{0:F2}")]
    public decimal? AvaliacaoMedia { get; set; }

    [Display(Name = "Disponível")]
    public bool Disponivel { get; set; }

    [Display(Name = "Total de Serviços")]
    public int TotalServicos { get; set; }

    [Display(Name = "Total de Avaliações")]
    public int TotalAvaliacoes { get; set; }
}

/// <summary>
/// ViewModel para busca/filtro de profissionais
/// </summary>
public class BuscarProfissionalViewModel
{
    [Display(Name = "Nome")]
    public string? Nome { get; set; }

    [Display(Name = "Cidade")]
    public string? Cidade { get; set; }

    [Display(Name = "Disponível")]
    public bool? Disponivel { get; set; }

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

