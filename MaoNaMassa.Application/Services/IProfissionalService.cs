using MaoNaMassa.Application.DTOs;

namespace MaoNaMassa.Application.Services;

public interface IProfissionalService
{
    Task<ProfissionalDTO> CriarPerfilProfissionalAsync(CreateProfissionalDTO dto);
    Task<ProfissionalDTO?> GetByUsuarioIdAsync(Guid usuarioId);
    Task<ProfissionalDTO?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProfissionalDTO>> GetDisponiveisAsync();
    Task AtualizarDisponibilidadeAsync(Guid id, bool disponivel);
}

