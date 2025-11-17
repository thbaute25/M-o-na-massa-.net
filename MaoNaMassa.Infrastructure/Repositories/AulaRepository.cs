using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaoNaMassa.Infrastructure.Repositories;

public class AulaRepository : IAulaRepository
{
    private readonly ApplicationDbContext _context;

    public AulaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Aula?> GetByIdAsync(Guid id)
    {
        return await _context.Aulas
            .Include(a => a.Curso)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Aula>> GetByCursoIdAsync(Guid cursoId)
    {
        return await _context.Aulas
            .Where(a => a.CursoId == cursoId)
            .OrderBy(a => a.Ordem)
            .ToListAsync();
    }

    public async Task<Aula> CreateAsync(Aula aula)
    {
        _context.Aulas.Add(aula);
        await _context.SaveChangesAsync();
        return aula;
    }

    public async Task UpdateAsync(Aula aula)
    {
        _context.Aulas.Update(aula);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var aula = await GetByIdAsync(id);
        if (aula != null)
        {
            _context.Aulas.Remove(aula);
            await _context.SaveChangesAsync();
        }
    }
}

