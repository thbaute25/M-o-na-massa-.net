using MaoNaMassa.Application.DTOs;

namespace MaoNaMassa.Application.Services;

public interface IServicoService
{
    Task<ServicoDTO> CriarServicoAsync(CreateServicoDTO dto);
    Task<ServicoDTO?> GetByIdAsync(Guid id);
    Task<IEnumerable<ServicoDTO>> GetByProfissionalIdAsync(Guid profissionalId);
    Task<IEnumerable<ServicoDTO>> BuscarServicosAsync(BuscarServicoDTO filtros);
}

