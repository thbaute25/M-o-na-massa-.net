using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

/// <summary>
/// ViewModel genérico para paginação
/// </summary>
public class PaginacaoViewModel
{
    [Display(Name = "Página Atual")]
    [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior que zero")]
    public int PaginaAtual { get; set; } = 1;

    [Display(Name = "Itens por Página")]
    [Range(1, 100, ErrorMessage = "Os itens por página devem estar entre 1 e 100")]
    public int ItensPorPagina { get; set; } = 10;

    [Display(Name = "Total de Itens")]
    public int TotalItens { get; set; }

    [Display(Name = "Total de Páginas")]
    public int TotalPaginas { get; set; }

    [Display(Name = "Tem Página Anterior")]
    public bool TemPaginaAnterior { get; set; }

    [Display(Name = "Tem Próxima Página")]
    public bool TemProximaPagina { get; set; }
}

/// <summary>
/// ViewModel genérico para resposta paginada
/// </summary>
public class PaginacaoResponseViewModel<T>
{
    public List<T> Itens { get; set; } = new();
    public PaginacaoViewModel Paginacao { get; set; } = new();
}

