using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Domain.Interfaces;

public interface IProfissionalRepository
{
    Task<Profissional?> GetByIdAsync(Guid id);
    Task<Profissional?> GetByUsuarioIdAsync(Guid usuarioId);
    Task<IEnumerable<Profissional>> GetAllAsync();
    Task<IEnumerable<Profissional>> GetDisponiveisAsync();
    Task<Profissional> CreateAsync(Profissional profissional);
    Task UpdateAsync(Profissional profissional);
    Task UpdateAvaliacaoMediaAsync(Guid id, decimal? avaliacaoMedia);
}

