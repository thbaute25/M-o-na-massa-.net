using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Domain.Interfaces;

public interface ICertificadoRepository
{
    Task<Certificado?> GetByIdAsync(Guid id);
    Task<Certificado?> GetByCodigoAsync(string codigo);
    Task<IEnumerable<Certificado>> GetByUsuarioIdAsync(Guid usuarioId);
    Task<IEnumerable<Certificado>> GetByCursoIdAsync(Guid cursoId);
    Task<Certificado> CreateAsync(Certificado certificado);
    Task<bool> UsuarioJaPossuiCertificadoAsync(Guid usuarioId, Guid cursoId);
}

