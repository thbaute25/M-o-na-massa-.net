using MaoNaMassa.Domain.Exceptions;

namespace MaoNaMassa.Domain.Entities;

public class Avaliacao
{
    public Guid Id { get; private set; }
    public Guid ServicoId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public int Nota { get; private set; }
    public string? Comentario { get; private set; }
    public DateTime Data { get; private set; }
    
    // Navegação
    public virtual Servico Servico { get; private set; } = null!;
    public virtual Usuario Usuario { get; private set; } = null!;

    // Construtor privado para EF Core
    private Avaliacao() { }

    public Avaliacao(
        Guid servicoId,
        Guid usuarioId,
        int nota,
        string? comentario = null)
    {
        Id = Guid.NewGuid();
        Data = DateTime.UtcNow;
        
        SetServicoId(servicoId);
        SetUsuarioId(usuarioId);
        SetNota(nota);
        SetComentario(comentario);
    }

    public void SetServicoId(Guid servicoId)
    {
        if (servicoId == Guid.Empty)
            throw new DomainException("ID do serviço é obrigatório.");

        ServicoId = servicoId;
    }

    public void SetUsuarioId(Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            throw new DomainException("ID do usuário é obrigatório.");

        UsuarioId = usuarioId;
    }

    public void SetNota(int nota)
    {
        if (nota < 1)
            throw new DomainException("Nota deve ser no mínimo 1.");
        
        if (nota > 5)
            throw new DomainException("Nota deve ser no máximo 5.");

        Nota = nota;
    }

    public void SetComentario(string? comentario)
    {
        if (!string.IsNullOrWhiteSpace(comentario))
        {
            if (comentario.Length > 500)
                throw new DomainException("Comentário deve ter no máximo 500 caracteres.");

            Comentario = comentario.Trim();
        }
        else
        {
            Comentario = null;
        }
    }

    public static Avaliacao Criar(Usuario usuario, Servico servico, int nota, string? comentario = null)
    {
        if (usuario == null)
            throw new DomainException("Usuário não pode ser nulo.");
        
        if (servico == null)
            throw new DomainException("Serviço não pode ser nulo.");

        return new Avaliacao(servico.Id, usuario.Id, nota, comentario);
    }
}

