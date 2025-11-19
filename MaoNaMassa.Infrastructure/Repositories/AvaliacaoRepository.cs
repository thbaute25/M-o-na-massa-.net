using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaoNaMassa.Infrastructure.Repositories;

public class AvaliacaoRepository : IAvaliacaoRepository
{
    private readonly ApplicationDbContext _context;

    public AvaliacaoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Avaliacao?> GetByIdAsync(Guid id)
    {
        return await _context.Avaliacoes
            .Include(a => a.Servico)
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Avaliacao>> GetByServicoIdAsync(Guid servicoId)
    {
        return await _context.Avaliacoes
            .Where(a => a.ServicoId == servicoId)
            .Include(a => a.Usuario)
            .OrderByDescending(a => a.Data)
            .ToListAsync();
    }

    public async Task<IEnumerable<Avaliacao>> GetByUsuarioIdAsync(Guid usuarioId)
    {
        return await _context.Avaliacoes
            .Where(a => a.UsuarioId == usuarioId)
            .Include(a => a.Servico)
            .OrderByDescending(a => a.Data)
            .ToListAsync();
    }

    public async Task<Avaliacao> CreateAsync(Avaliacao avaliacao)
    {
        _context.Avaliacoes.Add(avaliacao);
        await _context.SaveChangesAsync();
        return avaliacao;
    }

    public async Task<decimal?> GetAvaliacaoMediaByServicoIdAsync(Guid servicoId)
    {
        var avaliacoes = await _context.Avaliacoes
            .Where(a => a.ServicoId == servicoId)
            .ToListAsync();

        if (!avaliacoes.Any())
            return null;

        return avaliacoes.Average(a => (decimal)a.Nota);
    }

    public async Task<bool> UsuarioJaAvaliouServicoAsync(Guid usuarioId, Guid servicoId)
    {
        return await _context.Avaliacoes
            .AnyAsync(a => a.UsuarioId == usuarioId && a.ServicoId == servicoId);
    }
}

