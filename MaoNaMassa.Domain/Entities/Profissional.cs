using MaoNaMassa.Domain.Exceptions;

namespace MaoNaMassa.Domain.Entities;

public class Profissional
{
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public string Descricao { get; private set; }
    public decimal? AvaliacaoMedia { get; private set; }
    public bool Disponivel { get; private set; }
    
    // Navegação
    public virtual Usuario Usuario { get; private set; } = null!;
    public virtual ICollection<Servico> Servicos { get; private set; } = new List<Servico>();

    // Construtor privado para EF Core
    private Profissional() { }

    public Profissional(
        Guid usuarioId,
        string descricao)
    {
        Id = Guid.NewGuid();
        Disponivel = true;
        AvaliacaoMedia = null;
        
        SetUsuarioId(usuarioId);
        SetDescricao(descricao);
    }

    public void SetUsuarioId(Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            throw new DomainException("ID do usuário é obrigatório.");

        UsuarioId = usuarioId;
    }

    public void SetDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição do profissional é obrigatória.");
        
        if (descricao.Length < 20)
            throw new DomainException("Descrição deve ter no mínimo 20 caracteres.");
        
        if (descricao.Length > 1000)
            throw new DomainException("Descrição deve ter no máximo 1000 caracteres.");

        Descricao = descricao.Trim();
    }

    public void AtualizarAvaliacaoMedia(decimal? avaliacaoMedia)
    {
        if (avaliacaoMedia.HasValue)
        {
            if (avaliacaoMedia.Value < 0)
                throw new DomainException("Avaliação média não pode ser negativa.");
            
            if (avaliacaoMedia.Value > 5)
                throw new DomainException("Avaliação média não pode ser maior que 5.");
        }

        AvaliacaoMedia = avaliacaoMedia;
    }

    public void AlterarDisponibilidade(bool disponivel)
    {
        Disponivel = disponivel;
    }

    public void AdicionarServico(Servico servico)
    {
        if (servico == null)
            throw new DomainException("Serviço não pode ser nulo.");
        
        if (servico.ProfissionalId != Id)
            throw new DomainException("Serviço não pertence a este profissional.");

        Servicos.Add(servico);
    }

    public void RecalcularAvaliacaoMedia()
    {
        if (!Servicos.Any())
        {
            AvaliacaoMedia = null;
            return;
        }

        var todasAvaliacoes = Servicos
            .SelectMany(s => s.Avaliacoes)
            .Where(a => a.Nota > 0)
            .ToList();

        if (!todasAvaliacoes.Any())
        {
            AvaliacaoMedia = null;
            return;
        }

        AvaliacaoMedia = (decimal?)todasAvaliacoes.Average(a => a.Nota);
    }
}

