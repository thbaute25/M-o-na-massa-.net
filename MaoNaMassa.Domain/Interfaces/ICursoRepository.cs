using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Domain.Interfaces;

public interface ICursoRepository
{
    Task<Curso?> GetByIdAsync(Guid id);
    Task<IEnumerable<Curso>> GetAllAsync();
    Task<IEnumerable<Curso>> GetByAreaAsync(string area);
    Task<IEnumerable<Curso>> GetByNivelAsync(string nivel);
    Task<Curso> CreateAsync(Curso curso);
    Task UpdateAsync(Curso curso);
    Task DeleteAsync(Guid id);
}

