using MaoNaMassa.Application.DTOs;

namespace MaoNaMassa.Application.Services;

public interface IAvaliacaoService
{
    Task<AvaliacaoDTO> CriarAvaliacaoAsync(CreateAvaliacaoDTO dto);
    Task<IEnumerable<AvaliacaoDTO>> GetByServicoIdAsync(Guid servicoId);
    Task<decimal?> GetAvaliacaoMediaByServicoIdAsync(Guid servicoId);
}

