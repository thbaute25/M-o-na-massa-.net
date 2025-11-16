using MaoNaMassa.Domain.Exceptions;

namespace MaoNaMassa.Domain.Entities;

public class Aula
{
    public Guid Id { get; private set; }
    public Guid CursoId { get; private set; }
    public string Titulo { get; private set; }
    public string Conteudo { get; private set; }
    public int Ordem { get; private set; }
    
    // Navegação
    public virtual Curso Curso { get; private set; } = null!;

    // Construtor privado para EF Core
    private Aula() { }

    public Aula(
        Guid cursoId,
        string titulo,
        string conteudo,
        int ordem)
    {
        Id = Guid.NewGuid();
        
        SetCursoId(cursoId);
        SetTitulo(titulo);
        SetConteudo(conteudo);
        SetOrdem(ordem);
    }

    public void SetCursoId(Guid cursoId)
    {
        if (cursoId == Guid.Empty)
            throw new DomainException("ID do curso é obrigatório.");

        CursoId = cursoId;
    }

    public void SetTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Título da aula é obrigatório.");
        
        if (titulo.Length < 5)
            throw new DomainException("Título deve ter no mínimo 5 caracteres.");
        
        if (titulo.Length > 200)
            throw new DomainException("Título deve ter no máximo 200 caracteres.");

        Titulo = titulo.Trim();
    }

    public void SetConteudo(string conteudo)
    {
        if (string.IsNullOrWhiteSpace(conteudo))
            throw new DomainException("Conteúdo da aula é obrigatório.");
        
        if (conteudo.Length < 20)
            throw new DomainException("Conteúdo deve ter no mínimo 20 caracteres.");

        Conteudo = conteudo.Trim();
    }

    public void SetOrdem(int ordem)
    {
        if (ordem < 1)
            throw new DomainException("Ordem da aula deve ser maior que zero.");

        Ordem = ordem;
    }

    public void AtualizarOrdem(int novaOrdem)
    {
        SetOrdem(novaOrdem);
    }
}

