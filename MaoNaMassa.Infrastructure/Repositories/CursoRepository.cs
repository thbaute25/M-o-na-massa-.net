using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaoNaMassa.Infrastructure.Repositories;

public class CursoRepository : ICursoRepository
{
    private readonly ApplicationDbContext _context;

    public CursoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Curso?> GetByIdAsync(Guid id)
    {
        return await _context.Cursos
            .Include(c => c.Aulas)
            .Include(c => c.Quizzes)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Curso>> GetAllAsync()
    {
        return await _context.Cursos
            .Include(c => c.Aulas)
            .Include(c => c.Quizzes)
            .ToListAsync();
    }

    public async Task<IEnumerable<Curso>> GetByAreaAsync(string area)
    {
        return await _context.Cursos
            .Where(c => c.Area == area)
            .Include(c => c.Aulas)
            .Include(c => c.Quizzes)
            .ToListAsync();
    }

    public async Task<IEnumerable<Curso>> GetByNivelAsync(string nivel)
    {
        return await _context.Cursos
            .Where(c => c.Nivel == nivel)
            .Include(c => c.Aulas)
            .Include(c => c.Quizzes)
            .ToListAsync();
    }

    public async Task<Curso> CreateAsync(Curso curso)
    {
        _context.Cursos.Add(curso);
        await _context.SaveChangesAsync();
        return curso;
    }

    public async Task UpdateAsync(Curso curso)
    {
        _context.Cursos.Update(curso);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var curso = await GetByIdAsync(id);
        if (curso != null)
        {
            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
        }
    }
}

