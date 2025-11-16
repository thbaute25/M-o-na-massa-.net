using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Domain.Interfaces;

public interface IAulaRepository
{
    Task<Aula?> GetByIdAsync(Guid id);
    Task<IEnumerable<Aula>> GetByCursoIdAsync(Guid cursoId);
    Task<Aula> CreateAsync(Aula aula);
    Task UpdateAsync(Aula aula);
    Task DeleteAsync(Guid id);
}

