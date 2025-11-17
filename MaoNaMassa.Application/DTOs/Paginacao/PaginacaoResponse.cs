namespace MaoNaMassa.Application.DTOs.Paginacao;

/// <summary>
/// DTO para respostas paginadas
/// </summary>
public class PaginacaoResponse<T>
{
    public List<T> Itens { get; set; } = new();
    public int PaginaAtual { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalItens { get; set; }
    public int TotalPaginas { get; set; }
    public bool TemPaginaAnterior => PaginaAtual > 1;
    public bool TemProximaPagina => PaginaAtual < TotalPaginas;

    public PaginacaoResponse(List<T> itens, int paginaAtual, int tamanhoPagina, int totalItens)
    {
        Itens = itens;
        PaginaAtual = paginaAtual;
        TamanhoPagina = tamanhoPagina;
        TotalItens = totalItens;
        TotalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoPagina);
    }
}

