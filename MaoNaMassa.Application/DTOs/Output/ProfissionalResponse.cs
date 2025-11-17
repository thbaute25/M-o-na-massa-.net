namespace MaoNaMassa.Application.DTOs.Output;

/// <summary>
/// DTO de saída para informações do profissional
/// </summary>
public class ProfissionalResponse
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string EmailUsuario { get; set; } = string.Empty;
    public string CidadeUsuario { get; set; } = string.Empty;
    public string AreaInteresse { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal? AvaliacaoMedia { get; set; }
    public bool Disponivel { get; set; }
    public int TotalServicos { get; set; }
    public int TotalAvaliacoes { get; set; }
    public List<CertificadoBasicoResponse> Certificados { get; set; } = new();
}

/// <summary>
/// DTO básico de certificado para listagem
/// </summary>
public class CertificadoBasicoResponse
{
    public Guid Id { get; set; }
    public string TituloCurso { get; set; } = string.Empty;
    public decimal NotaFinal { get; set; }
    public DateTime DataConclusao { get; set; }
}

