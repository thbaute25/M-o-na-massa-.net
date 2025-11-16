using MaoNaMassa.Domain.Exceptions;

namespace MaoNaMassa.Domain.Entities;

public class Certificado
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid CursoId { get; private set; }
    public decimal NotaFinal { get; private set; }
    public DateTime DataConclusao { get; private set; }
    public string CodigoCertificado { get; private set; }
    
    // Navegação
    public virtual Usuario Usuario { get; private set; } = null!;
    public virtual Curso Curso { get; private set; } = null!;

    // Construtor privado para EF Core
    private Certificado() { }

    public Certificado(
        Guid usuarioId,
        Guid cursoId,
        decimal notaFinal)
    {
        Id = Guid.NewGuid();
        DataConclusao = DateTime.UtcNow;
        
        SetUsuarioId(usuarioId);
        SetCursoId(cursoId);
        SetNotaFinal(notaFinal);
        CodigoCertificado = GerarCodigoCertificado();
    }

    public void SetUsuarioId(Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            throw new DomainException("ID do usuário é obrigatório.");

        UsuarioId = usuarioId;
    }

    public void SetCursoId(Guid cursoId)
    {
        if (cursoId == Guid.Empty)
            throw new DomainException("ID do curso é obrigatório.");

        CursoId = cursoId;
    }

    public void SetNotaFinal(decimal notaFinal)
    {
        if (notaFinal < 0)
            throw new DomainException("Nota final não pode ser negativa.");
        
        if (notaFinal > 100)
            throw new DomainException("Nota final não pode ser maior que 100.");

        NotaFinal = notaFinal;
    }

    private string GerarCodigoCertificado()
    {
        // Gera um código único baseado em timestamp e GUID
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var guid = Id.ToString("N").Substring(0, 8).ToUpperInvariant();
        return $"CERT-{timestamp}-{guid}";
    }

    public bool EstaAprovado()
    {
        // Considera aprovado se nota >= 70
        return NotaFinal >= 70;
    }

    public static Certificado Criar(Usuario usuario, Curso curso, decimal notaFinal)
    {
        if (usuario == null)
            throw new DomainException("Usuário não pode ser nulo.");
        
        if (curso == null)
            throw new DomainException("Curso não pode ser nulo.");

        return new Certificado(usuario.Id, curso.Id, notaFinal);
    }
}

