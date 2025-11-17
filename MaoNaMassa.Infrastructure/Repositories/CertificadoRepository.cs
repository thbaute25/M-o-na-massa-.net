using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaoNaMassa.Infrastructure.Repositories;

public class CertificadoRepository : ICertificadoRepository
{
    private readonly ApplicationDbContext _context;

    public CertificadoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Certificado?> GetByIdAsync(Guid id)
    {
        return await _context.Certificados
            .Include(c => c.Usuario)
            .Include(c => c.Curso)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Certificado?> GetByCodigoAsync(string codigo)
    {
        return await _context.Certificados
            .Include(c => c.Usuario)
            .Include(c => c.Curso)
            .FirstOrDefaultAsync(c => c.CodigoCertificado == codigo);
    }

    public async Task<IEnumerable<Certificado>> GetByUsuarioIdAsync(Guid usuarioId)
    {
        return await _context.Certificados
            .Where(c => c.UsuarioId == usuarioId)
            .Include(c => c.Curso)
            .ToListAsync();
    }

    public async Task<IEnumerable<Certificado>> GetByCursoIdAsync(Guid cursoId)
    {
        return await _context.Certificados
            .Where(c => c.CursoId == cursoId)
            .Include(c => c.Usuario)
            .ToListAsync();
    }

    public async Task<Certificado> CreateAsync(Certificado certificado)
    {
        _context.Certificados.Add(certificado);
        await _context.SaveChangesAsync();
        return certificado;
    }

    public async Task<bool> UsuarioJaPossuiCertificadoAsync(Guid usuarioId, Guid cursoId)
    {
        return await _context.Certificados
            .AnyAsync(c => c.UsuarioId == usuarioId && c.CursoId == cursoId);
    }
}

