using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Domain.Interfaces;

public interface IAvaliacaoRepository
{
    Task<Avaliacao?> GetByIdAsync(Guid id);
    Task<IEnumerable<Avaliacao>> GetByServicoIdAsync(Guid servicoId);
    Task<IEnumerable<Avaliacao>> GetByUsuarioIdAsync(Guid usuarioId);
    Task<Avaliacao> CreateAsync(Avaliacao avaliacao);
    Task<decimal?> GetAvaliacaoMediaByServicoIdAsync(Guid servicoId);
    Task<bool> UsuarioJaAvaliouServicoAsync(Guid usuarioId, Guid servicoId);
}

