namespace MaoNaMassa.Application.DTOs;

public class AvaliacaoDTO
{
    public Guid Id { get; set; }
    public Guid ServicoId { get; set; }
    public Guid UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public int Nota { get; set; }
    public string? Comentario { get; set; }
    public DateTime Data { get; set; }
}

public class CreateAvaliacaoDTO
{
    public Guid ServicoId { get; set; }
    public Guid UsuarioId { get; set; }
    public int Nota { get; set; }
    public string? Comentario { get; set; }
}

