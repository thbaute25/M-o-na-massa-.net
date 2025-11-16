using MaoNaMassa.Domain.Exceptions;

namespace MaoNaMassa.Domain.Entities;

public class Servico
{
    public Guid Id { get; private set; }
    public Guid ProfissionalId { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public string Cidade { get; private set; }
    public decimal? Preco { get; private set; }
    public DateTime DataPublicacao { get; private set; }
    
    // Navegação
    public virtual Profissional Profissional { get; private set; } = null!;
    public virtual ICollection<Avaliacao> Avaliacoes { get; private set; } = new List<Avaliacao>();

    // Construtor privado para EF Core
    private Servico() { }

    public Servico(
        Guid profissionalId,
        string titulo,
        string descricao,
        string cidade,
        decimal? preco = null)
    {
        Id = Guid.NewGuid();
        DataPublicacao = DateTime.UtcNow;
        
        SetProfissionalId(profissionalId);
        SetTitulo(titulo);
        SetDescricao(descricao);
        SetCidade(cidade);
        SetPreco(preco);
    }

    public void SetProfissionalId(Guid profissionalId)
    {
        if (profissionalId == Guid.Empty)
            throw new DomainException("ID do profissional é obrigatório.");

        ProfissionalId = profissionalId;
    }

    public void SetTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Título do serviço é obrigatório.");
        
        if (titulo.Length < 5)
            throw new DomainException("Título deve ter no mínimo 5 caracteres.");
        
        if (titulo.Length > 200)
            throw new DomainException("Título deve ter no máximo 200 caracteres.");

        Titulo = titulo.Trim();
    }

    public void SetDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição do serviço é obrigatória.");
        
        if (descricao.Length < 20)
            throw new DomainException("Descrição deve ter no mínimo 20 caracteres.");
        
        if (descricao.Length > 1000)
            throw new DomainException("Descrição deve ter no máximo 1000 caracteres.");

        Descricao = descricao.Trim();
    }

    public void SetCidade(string cidade)
    {
        if (string.IsNullOrWhiteSpace(cidade))
            throw new DomainException("Cidade é obrigatória.");
        
        if (cidade.Length > 100)
            throw new DomainException("Cidade deve ter no máximo 100 caracteres.");

        Cidade = cidade.Trim();
    }

    public void SetPreco(decimal? preco)
    {
        if (preco.HasValue && preco.Value < 0)
            throw new DomainException("Preço não pode ser negativo.");

        Preco = preco;
    }

    public void AdicionarAvaliacao(Avaliacao avaliacao)
    {
        if (avaliacao == null)
            throw new DomainException("Avaliação não pode ser nula.");
        
        if (avaliacao.ServicoId != Id)
            throw new DomainException("Avaliação não pertence a este serviço.");

        Avaliacoes.Add(avaliacao);
    }

    public decimal? CalcularAvaliacaoMedia()
    {
        if (!Avaliacoes.Any())
            return null;

        return (decimal?)Avaliacoes.Average(a => a.Nota);
    }

    public int ObterTotalAvaliacoes()
    {
        return Avaliacoes.Count;
    }
}

