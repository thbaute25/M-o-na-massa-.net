namespace MaoNaMassa.Application.DTOs.Output;

/// <summary>
/// DTO de saída para informações do certificado
/// </summary>
public class CertificadoResponse
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid CursoId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string TituloCurso { get; set; } = string.Empty;
    public decimal NotaFinal { get; set; }
    public DateTime DataConclusao { get; set; }
    public string CodigoCertificado { get; set; } = string.Empty;
    public bool Aprovado { get; set; }
    public string Status { get; set; } = string.Empty; // "Aprovado", "Reprovado"
}

