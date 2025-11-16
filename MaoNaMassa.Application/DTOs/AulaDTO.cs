namespace MaoNaMassa.Application.DTOs;

public class AulaDTO
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public int Ordem { get; set; }
}

public class CreateAulaDTO
{
    public Guid CursoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public int Ordem { get; set; }
}

