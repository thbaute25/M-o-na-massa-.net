namespace MaoNaMassa.Application.DTOs.Paginacao;

/// <summary>
/// DTO para requisições paginadas
/// </summary>
public class PaginacaoRequest
{
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;
    public string? OrdenarPor { get; set; }
    public bool OrdenarDescendente { get; set; } = false;

    public int Skip => (Pagina - 1) * TamanhoPagina;
    public int Take => TamanhoPagina;
}

