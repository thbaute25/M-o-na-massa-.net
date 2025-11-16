using MaoNaMassa.Application.DTOs;

namespace MaoNaMassa.Application.Services;

public interface ICertificadoService
{
    Task<CertificadoDTO?> GerarCertificadoAsync(Guid usuarioId, Guid cursoId);
    Task<CertificadoDTO?> GetByIdAsync(Guid id);
    Task<CertificadoDTO?> GetByCodigoAsync(string codigo);
    Task<IEnumerable<CertificadoDTO>> GetByUsuarioIdAsync(Guid usuarioId);
    Task<bool> UsuarioPossuiCertificadoAsync(Guid usuarioId, Guid cursoId);
}

