using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaoNaMassa.Infrastructure.Repositories;

public class ServicoRepository : IServicoRepository
{
    private readonly ApplicationDbContext _context;

    public ServicoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Servico?> GetByIdAsync(Guid id)
    {
        return await _context.Servicos
            .Include(s => s.Profissional)
                .ThenInclude(p => p!.Usuario)
            .Include(s => s.Avaliacoes)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Servico>> GetAllAsync()
    {
        return await _context.Servicos
            .Include(s => s.Profissional)
                .ThenInclude(p => p!.Usuario)
            .Include(s => s.Avaliacoes)
            .ToListAsync();
    }

    public async Task<IEnumerable<Servico>> GetByProfissionalIdAsync(Guid profissionalId)
    {
        return await _context.Servicos
            .Where(s => s.ProfissionalId == profissionalId)
            .Include(s => s.Profissional)
                .ThenInclude(p => p!.Usuario)
            .Include(s => s.Avaliacoes)
            .ToListAsync();
    }

    public async Task<IEnumerable<Servico>> GetByCidadeAsync(string cidade)
    {
        return await _context.Servicos
            .Where(s => s.Cidade == cidade)
            .Include(s => s.Profissional)
                .ThenInclude(p => p!.Usuario)
            .Include(s => s.Avaliacoes)
            .ToListAsync();
    }

    public async Task<IEnumerable<Servico>> SearchAsync(string? cidade, string? termo)
    {
        var query = _context.Servicos
            .Include(s => s.Profissional)
                .ThenInclude(p => p!.Usuario)
            .Include(s => s.Avaliacoes)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(cidade))
        {
            query = query.Where(s => s.Cidade.Contains(cidade));
        }

        if (!string.IsNullOrWhiteSpace(termo))
        {
            query = query.Where(s => 
                s.Titulo.Contains(termo) || 
                s.Descricao.Contains(termo));
        }

        return await query.ToListAsync();
    }

    public async Task<Servico> CreateAsync(Servico servico)
    {
        _context.Servicos.Add(servico);
        await _context.SaveChangesAsync();
        return servico;
    }

    public async Task UpdateAsync(Servico servico)
    {
        _context.Servicos.Update(servico);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var servico = await GetByIdAsync(id);
        if (servico != null)
        {
            _context.Servicos.Remove(servico);
            await _context.SaveChangesAsync();
        }
    }
}

