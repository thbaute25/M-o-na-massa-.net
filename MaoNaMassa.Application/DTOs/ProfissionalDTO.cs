namespace MaoNaMassa.Application.DTOs;

public class ProfissionalDTO
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string EmailUsuario { get; set; } = string.Empty;
    public string CidadeUsuario { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal? AvaliacaoMedia { get; set; }
    public bool Disponivel { get; set; }
    public int TotalServicos { get; set; }
    public int TotalAvaliacoes { get; set; }
}

public class CreateProfissionalDTO
{
    public Guid UsuarioId { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

