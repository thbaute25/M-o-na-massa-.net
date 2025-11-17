using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

/// <summary>
/// ViewModel para exibição de dados do certificado
/// </summary>
public class CertificadoViewModel
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid CursoId { get; set; }

    [Display(Name = "Nome do Usuário")]
    public string NomeUsuario { get; set; } = string.Empty;

    [Display(Name = "Título do Curso")]
    public string TituloCurso { get; set; } = string.Empty;

    [Display(Name = "Nota Final")]
    [DisplayFormat(DataFormatString = "{0:F2}")]
    public decimal NotaFinal { get; set; }

    [Display(Name = "Data de Conclusão")]
    [DataType(DataType.Date)]
    public DateTime DataConclusao { get; set; }

    [Display(Name = "Código do Certificado")]
    public string CodigoCertificado { get; set; } = string.Empty;

    [Display(Name = "Aprovado")]
    public bool Aprovado { get; set; }
}

/// <summary>
/// ViewModel para verificação de certificado
/// </summary>
public class VerificarCertificadoViewModel
{
    [Required(ErrorMessage = "O código do certificado é obrigatório")]
    [StringLength(50, ErrorMessage = "O código do certificado deve ter no máximo 50 caracteres")]
    [Display(Name = "Código do Certificado")]
    public string Codigo { get; set; } = string.Empty;

    // Resultado da verificação
    public CertificadoViewModel? Certificado { get; set; }
    public bool CertificadoValido { get; set; }
    public string? Mensagem { get; set; }
}

