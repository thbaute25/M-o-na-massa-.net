using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaoNaMassa.Infrastructure.Repositories;

public class ProfissionalRepository : IProfissionalRepository
{
    private readonly ApplicationDbContext _context;

    public ProfissionalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Profissional?> GetByIdAsync(Guid id)
    {
        return await _context.Profissionais
            .Include(p => p.Usuario)
            .Include(p => p.Servicos)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Profissional?> GetByUsuarioIdAsync(Guid usuarioId)
    {
        return await _context.Profissionais
            .Include(p => p.Usuario)
            .Include(p => p.Servicos)
            .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
    }

    public async Task<IEnumerable<Profissional>> GetAllAsync()
    {
        return await _context.Profissionais
            .Include(p => p.Usuario)
            .Include(p => p.Servicos)
            .ToListAsync();
    }

    public async Task<IEnumerable<Profissional>> GetDisponiveisAsync()
    {
        return await _context.Profissionais
            .Where(p => p.Disponivel)
            .Include(p => p.Usuario)
            .Include(p => p.Servicos)
            .ToListAsync();
    }

    public async Task<Profissional> CreateAsync(Profissional profissional)
    {
        _context.Profissionais.Add(profissional);
        await _context.SaveChangesAsync();
        return profissional;
    }

    public async Task UpdateAsync(Profissional profissional)
    {
        _context.Profissionais.Update(profissional);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAvaliacaoMediaAsync(Guid id, decimal? avaliacaoMedia)
    {
        var profissional = await GetByIdAsync(id);
        if (profissional != null)
        {
            profissional.AtualizarAvaliacaoMedia(avaliacaoMedia);
            await _context.SaveChangesAsync();
        }
    }
}

